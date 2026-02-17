using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Extension methods for registering role authorization services.
/// Follows the same pattern as <see cref="SubscriptionAuthorizationExtensions"/>.
/// </summary>
public static class RoleAuthorizationExtensions
{
    /// <summary>
    /// Register the shared role claims HTTP client and supporting services.
    /// Call this in each service's Infrastructure DependencyInjection.
    /// </summary>
    /// <example>
    /// services.AddRoleClaimsClient();
    /// </example>
    public static IServiceCollection AddRoleClaimsClient(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddHttpClient<IRoleClaimsClient, RoleClaimsHttpClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    /// <summary>
    /// Use role enrichment middleware.
    /// Must be called after UseAuthentication() and before UseAuthorization().
    /// </summary>
    /// <example>
    /// app.UseAuthentication();
    /// app.UseRoleEnrichment();
    /// app.UseSubscriptionEnrichment(); // optional
    /// app.UseAuthorization();
    /// </example>
    public static IApplicationBuilder UseRoleEnrichment(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RoleEnrichmentMiddleware>();
    }
}
