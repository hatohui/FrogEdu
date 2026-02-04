using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Middleware that enriches authenticated requests with subscription claims
/// fetched from the Subscription microservice.
///
/// This middleware:
/// - Only runs for authenticated users
/// - Fetches subscription claims using the user's ID
/// - Caches claims to minimize service calls
/// - Adds claims to HttpContext for downstream use
/// </summary>
public sealed class SubscriptionEnrichmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SubscriptionEnrichmentMiddleware> _logger;

    // Cache duration in minutes - subscription status doesn't change frequently
    private const int CacheDurationMinutes = 5;

    public SubscriptionEnrichmentMiddleware(
        RequestDelegate next,
        ILogger<SubscriptionEnrichmentMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ISubscriptionClaimsClient subscriptionClient,
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
            var userId = GetUserId(context);

            if (userId.HasValue)
            {
                var claims = await GetOrFetchSubscriptionClaims(
                    subscriptionClient,
                    cache,
                    userId.Value,
                    context.RequestAborted
                );

                context.SetSubscriptionClaims(claims);

                // Also add claims to ClaimsPrincipal for policy-based authorization
                EnrichClaimsPrincipal(context, claims);

                _logger.LogDebug(
                    "Enriched request for user {UserId} with subscription plan: {Plan}, active: {IsActive}",
                    userId,
                    claims.Plan,
                    claims.HasActiveSubscription
                );
            }
        }
        catch (Exception ex)
        {
            // Don't fail the request if subscription enrichment fails
            // Just log and continue with default (free) plan
            _logger.LogWarning(
                ex,
                "Failed to enrich request with subscription claims. Continuing with default plan."
            );
        }

        await _next(context);
    }

    private static Guid? GetUserId(HttpContext context)
    {
        // Try to get user ID from various claim types
        // 1. Standard "sub" claim from Cognito
        var subClaim =
            context.User.FindFirst("sub")?.Value
            ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(subClaim) && Guid.TryParse(subClaim, out var subGuid))
        {
            return subGuid;
        }

        // 2. Try custom:user_id claim if present
        var userIdClaim = context.User.FindFirst("custom:user_id")?.Value;

        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userIdGuid))
        {
            return userIdGuid;
        }

        return null;
    }

    private async Task<SubscriptionClaimsDto> GetOrFetchSubscriptionClaims(
        ISubscriptionClaimsClient client,
        IMemoryCache cache,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var cacheKey = $"subscription_claims_{userId}";

        if (cache.TryGetValue<SubscriptionClaimsDto>(cacheKey, out var cachedClaims))
        {
            _logger.LogDebug("Using cached subscription claims for user {UserId}", userId);
            return cachedClaims!;
        }

        _logger.LogDebug("Fetching subscription claims from service for user {UserId}", userId);

        var claims = await client.GetSubscriptionClaimsAsync(userId, cancellationToken);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheDurationMinutes))
            .SetSize(1);

        cache.Set(cacheKey, claims, cacheOptions);

        return claims;
    }

    private static void EnrichClaimsPrincipal(HttpContext context, SubscriptionClaimsDto claims)
    {
        if (context.User.Identity is ClaimsIdentity identity)
        {
            // Remove existing subscription claims if any
            var existingPlanClaim = identity.FindFirst(SubscriptionConstants.PlanClaimType);
            if (existingPlanClaim != null)
                identity.RemoveClaim(existingPlanClaim);

            var existingExpiresClaim = identity.FindFirst(SubscriptionConstants.ExpiresAtClaimType);
            if (existingExpiresClaim != null)
                identity.RemoveClaim(existingExpiresClaim);

            var existingActiveClaim = identity.FindFirst(
                SubscriptionConstants.HasActiveSubscriptionClaimType
            );
            if (existingActiveClaim != null)
                identity.RemoveClaim(existingActiveClaim);

            // Add fresh subscription claims
            identity.AddClaim(new Claim(SubscriptionConstants.PlanClaimType, claims.Plan));
            identity.AddClaim(
                new Claim(SubscriptionConstants.ExpiresAtClaimType, claims.ExpiresAt.ToString())
            );
            identity.AddClaim(
                new Claim(
                    SubscriptionConstants.HasActiveSubscriptionClaimType,
                    claims.HasActiveSubscription.ToString().ToLowerInvariant()
                )
            );
        }
    }
}
