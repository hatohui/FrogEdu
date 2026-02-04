using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Extension methods for registering subscription authorization services
/// </summary>
public static class SubscriptionAuthorizationExtensions
{
    /// <summary>
    /// Add subscription authorization services including handlers and policies.
    /// Call this after AddAuthorization().
    /// </summary>
    /// <example>
    /// services.AddAuthorization();
    /// services.AddSubscriptionAuthorization();
    /// </example>
    public static IServiceCollection AddSubscriptionAuthorization(this IServiceCollection services)
    {
        // Register IHttpContextAccessor if not already registered
        services.AddHttpContextAccessor();

        // Register memory cache for subscription claims caching
        services.AddMemoryCache();

        // Register authorization handlers
        services.AddScoped<IAuthorizationHandler, ActiveSubscriptionAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, SubscriptionPlanAuthorizationHandler>();

        // Configure authorization policies
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                SubscriptionConstants.Policies.RequireActiveSubscription,
                policy => policy.Requirements.Add(new ActiveSubscriptionRequirement())
            )
            .AddPolicy(
                SubscriptionConstants.Policies.RequireProPlan,
                policy =>
                    policy.Requirements.Add(
                        new SubscriptionPlanRequirement(SubscriptionConstants.Plans.Pro)
                    )
            )
            .AddPolicy(
                SubscriptionConstants.Policies.RequireEnterprisePlan,
                policy =>
                    policy.Requirements.Add(
                        new SubscriptionPlanRequirement(SubscriptionConstants.Plans.Enterprise)
                    )
            );

        return services;
    }

    /// <summary>
    /// Add subscription authorization with custom plan policies.
    /// Call this after AddAuthorization().
    /// </summary>
    /// <param name="additionalPlans">Additional plan names to create policies for</param>
    /// <example>
    /// services.AddSubscriptionAuthorization("premium", "business");
    /// // Creates policies: RequirePlan:premium, RequirePlan:business
    /// </example>
    public static IServiceCollection AddSubscriptionAuthorization(
        this IServiceCollection services,
        params string[] additionalPlans
    )
    {
        services.AddSubscriptionAuthorization();

        // Add custom plan policies
        var authBuilder = services.AddAuthorizationBuilder();

        foreach (var plan in additionalPlans)
        {
            authBuilder.AddPolicy(
                $"RequirePlan:{plan}",
                policy => policy.Requirements.Add(new SubscriptionPlanRequirement(plan))
            );
        }

        return services;
    }

    /// <summary>
    /// Use subscription enrichment middleware.
    /// Must be called after UseAuthentication() and before UseAuthorization().
    /// </summary>
    /// <example>
    /// app.UseAuthentication();
    /// app.UseSubscriptionEnrichment();
    /// app.UseAuthorization();
    /// </example>
    public static IApplicationBuilder UseSubscriptionEnrichment(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SubscriptionEnrichmentMiddleware>();
    }
}
