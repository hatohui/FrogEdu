using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Middleware that enriches authenticated requests with role claims
/// fetched from the User microservice.
///
/// This middleware:
/// - Only runs for authenticated users
/// - Fetches role claims using the user's Cognito sub
/// - Caches claims to minimize service calls (5 min)
/// - Adds ClaimTypes.Role to ClaimsPrincipal for [Authorize(Roles = "...")] support
/// - Stores role claims in HttpContext.Items for programmatic access
///
/// Must be registered AFTER UseAuthentication() and BEFORE UseAuthorization().
/// </summary>
public sealed class RoleEnrichmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleEnrichmentMiddleware> _logger;

    private const int CacheDurationMinutes = 5;
    private const string RoleClaimsKey = "RoleClaims";

    public RoleEnrichmentMiddleware(RequestDelegate next, ILogger<RoleEnrichmentMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IRoleClaimsClient roleClient,
        IMemoryCache cache
    )
    {
        // Skip if user is not authenticated
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        try
        {
            var cognitoSub = GetCognitoSub(context);

            if (!string.IsNullOrEmpty(cognitoSub))
            {
                // Self-healing: Check if the JWT already carries a valid role via custom:role
                // (mapped to ClaimTypes.Role by OnTokenValidated in AuthenticationExtensions).
                // If present, trust it and skip the HTTP call to the User service entirely.
                // This is critical for Lambda deployments where services cannot reach each other.
                var existingRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

                if (
                    !string.IsNullOrEmpty(existingRole)
                    && !existingRole.Equals(
                        RoleConstants.Student,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    // JWT already has a meaningful role (Admin/Teacher) — trust it
                    var jwtRoleClaims = new RoleClaimsDto
                    {
                        CognitoSub = cognitoSub,
                        Role = char.ToUpper(existingRole[0]) + existingRole[1..].ToLower(),
                    };

                    // Enrich HttpContext.Items and normalize the principal's Role claim so
                    // [Authorize(Roles = "Admin")] works reliably even when the JWT used
                    // a lowercase role value (e.g. "admin").
                    context.Items[RoleClaimsKey] = jwtRoleClaims;
                    EnrichClaimsPrincipal(context, jwtRoleClaims);

                    _logger.LogDebug(
                        "Self-healing: Using JWT custom:role '{Role}' for user {CognitoSub}, skipping User service call",
                        jwtRoleClaims.Role,
                        cognitoSub
                    );

                    await _next(context);
                    return;
                }

                // No authoritative role in JWT — try fetching from User service
                var roleClaims = await GetOrFetchRoleClaims(
                    roleClient,
                    cache,
                    cognitoSub,
                    context.RequestAborted
                );

                // Only override the JWT role if we got a real (non-default) result from the User service.
                // If the HTTP call failed (returns Student default), preserve whatever the JWT had.
                if (roleClaims.UserId.HasValue)
                {
                    // Successfully fetched from User service — this is authoritative
                    context.Items[RoleClaimsKey] = roleClaims;
                    EnrichClaimsPrincipal(context, roleClaims);

                    _logger.LogDebug(
                        "Enriched request for user {CognitoSub} with role: {Role} (from User service)",
                        cognitoSub,
                        roleClaims.Role
                    );
                }
                else if (!string.IsNullOrEmpty(existingRole))
                {
                    // HTTP call failed but JWT has a role (even Student) — keep it
                    var fallbackClaims = new RoleClaimsDto
                    {
                        CognitoSub = cognitoSub,
                        Role = char.ToUpper(existingRole[0]) + existingRole[1..].ToLower(),
                    };
                    context.Items[RoleClaimsKey] = fallbackClaims;

                    _logger.LogDebug(
                        "Self-healing: User service unreachable, using JWT role '{Role}' for {CognitoSub}",
                        fallbackClaims.Role,
                        cognitoSub
                    );
                }
                else
                {
                    // No role available anywhere — use the default from the failed call
                    context.Items[RoleClaimsKey] = roleClaims;
                    EnrichClaimsPrincipal(context, roleClaims);

                    _logger.LogWarning(
                        "No role found for user {CognitoSub} — defaulting to {Role}. "
                            + "User should call GET /me on the User service to sync their role to Cognito.",
                        cognitoSub,
                        roleClaims.Role
                    );
                }
            }
        }
        catch (Exception ex)
        {
            // Don't fail the request if role enrichment fails.
            // Check if JWT already has a role claim from OnTokenValidated.
            var existingRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(existingRole))
            {
                _logger.LogWarning(
                    ex,
                    "Role enrichment failed, but JWT has role '{Role}' — keeping it.",
                    existingRole
                );
            }
            else
            {
                _logger.LogWarning(
                    ex,
                    "Failed to enrich request with role claims. User will not have role-based access."
                );
            }
        }

        await _next(context);
    }

    private static string? GetCognitoSub(HttpContext context)
    {
        return context.User.FindFirst("sub")?.Value
            ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    private async Task<RoleClaimsDto> GetOrFetchRoleClaims(
        IRoleClaimsClient client,
        IMemoryCache cache,
        string cognitoSub,
        CancellationToken cancellationToken
    )
    {
        var cacheKey = $"role_claims_{cognitoSub}";

        if (cache.TryGetValue<RoleClaimsDto>(cacheKey, out var cachedClaims))
        {
            _logger.LogDebug("Using cached role claims for user {CognitoSub}", cognitoSub);
            return cachedClaims!;
        }

        _logger.LogDebug("Fetching role claims from User service for {CognitoSub}", cognitoSub);

        var claims = await client.GetRoleClaimsAsync(cognitoSub, cancellationToken);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheDurationMinutes))
            .SetSize(1);

        cache.Set(cacheKey, claims, cacheOptions);

        return claims;
    }

    private static void EnrichClaimsPrincipal(HttpContext context, RoleClaimsDto roleClaims)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
            return;

        // Remove any existing Role claims (e.g., stale custom:role mapping)
        var existingRoleClaims = identity.FindAll(ClaimTypes.Role).ToList();
        foreach (var claim in existingRoleClaims)
        {
            identity.RemoveClaim(claim);
        }

        // Add the authoritative role claim from the User service database
        identity.AddClaim(new Claim(ClaimTypes.Role, roleClaims.Role));

        // Also add a custom claim with the internal user ID for convenience
        if (roleClaims.UserId.HasValue)
        {
            var existingUserIdClaim = identity.FindFirst("frogedu:user_id");
            if (existingUserIdClaim != null)
            {
                identity.RemoveClaim(existingUserIdClaim);
            }

            identity.AddClaim(new Claim("frogedu:user_id", roleClaims.UserId.Value.ToString()));
        }
    }

    /// <summary>
    /// Get role claims from HttpContext.Items (set by this middleware)
    /// </summary>
    public static RoleClaimsDto? GetRoleClaims(HttpContext context)
    {
        return context.Items.TryGetValue(RoleClaimsKey, out var value)
            ? value as RoleClaimsDto
            : null;
    }
}

/// <summary>
/// Extension methods for accessing role claims from HttpContext
/// </summary>
public static class RoleClaimsExtensions
{
    /// <summary>
    /// Get role claims set by <see cref="RoleEnrichmentMiddleware"/>
    /// </summary>
    public static RoleClaimsDto? GetRoleClaims(this HttpContext context)
    {
        return RoleEnrichmentMiddleware.GetRoleClaims(context);
    }

    /// <summary>
    /// Get the user's role name from the enriched claims
    /// </summary>
    public static string GetUserRole(this HttpContext context)
    {
        return context.GetRoleClaims()?.Role ?? RoleConstants.Student;
    }
}
