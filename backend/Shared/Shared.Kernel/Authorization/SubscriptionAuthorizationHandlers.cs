using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Authorization requirement for active subscription
/// </summary>
public sealed class ActiveSubscriptionRequirement : IAuthorizationRequirement { }

/// <summary>
/// Authorization requirement for specific subscription plan
/// </summary>
public sealed class SubscriptionPlanRequirement : IAuthorizationRequirement
{
    public string RequiredPlan { get; }

    public SubscriptionPlanRequirement(string requiredPlan)
    {
        RequiredPlan = requiredPlan;
    }
}

/// <summary>
/// Authorization handler for active subscription requirement.
/// Checks if the user has any active paid subscription.
/// </summary>
public sealed class ActiveSubscriptionAuthorizationHandler
    : AuthorizationHandler<ActiveSubscriptionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ActiveSubscriptionAuthorizationHandler> _logger;

    public ActiveSubscriptionAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<ActiveSubscriptionAuthorizationHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveSubscriptionRequirement requirement
    )
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            _logger.LogWarning("HttpContext is null in subscription authorization handler");
            return Task.CompletedTask;
        }

        var claims = httpContext.GetSubscriptionClaims();

        if (claims is null)
        {
            _logger.LogDebug(
                "No subscription claims found. User may not be authenticated or middleware not configured."
            );
            return Task.CompletedTask;
        }

        if (claims.HasActiveSubscription && claims.IsPaidSubscriber)
        {
            _logger.LogDebug(
                "User {UserId} has active subscription with plan {Plan}",
                claims.UserId,
                claims.Plan
            );
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogDebug(
                "User {UserId} does not have active paid subscription. Plan: {Plan}, Active: {Active}",
                claims.UserId,
                claims.Plan,
                claims.HasActiveSubscription
            );
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// Authorization handler for specific subscription plan requirement.
/// Checks if the user has the required plan or higher.
/// </summary>
public sealed class SubscriptionPlanAuthorizationHandler
    : AuthorizationHandler<SubscriptionPlanRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SubscriptionPlanAuthorizationHandler> _logger;

    // Plan hierarchy (higher index = higher tier)
    private static readonly string[] PlanHierarchy =
    [
        SubscriptionConstants.Plans.Free,
        SubscriptionConstants.Plans.Pro,
        SubscriptionConstants.Plans.Enterprise,
    ];

    public SubscriptionPlanAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<SubscriptionPlanAuthorizationHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SubscriptionPlanRequirement requirement
    )
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            _logger.LogWarning("HttpContext is null in subscription plan authorization handler");
            return Task.CompletedTask;
        }

        var claims = httpContext.GetSubscriptionClaims();

        if (claims is null)
        {
            _logger.LogDebug("No subscription claims found for plan check");
            return Task.CompletedTask;
        }

        if (!claims.HasActiveSubscription)
        {
            _logger.LogDebug(
                "User {UserId} does not have active subscription for plan check",
                claims.UserId
            );
            return Task.CompletedTask;
        }

        if (MeetsPlanRequirement(claims.Plan, requirement.RequiredPlan))
        {
            _logger.LogDebug(
                "User {UserId} with plan {UserPlan} meets requirement for {RequiredPlan}",
                claims.UserId,
                claims.Plan,
                requirement.RequiredPlan
            );
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogDebug(
                "User {UserId} with plan {UserPlan} does not meet requirement for {RequiredPlan}",
                claims.UserId,
                claims.Plan,
                requirement.RequiredPlan
            );
        }

        return Task.CompletedTask;
    }

    private static bool MeetsPlanRequirement(string userPlan, string requiredPlan)
    {
        var userPlanIndex = Array.FindIndex(
            PlanHierarchy,
            p => p.Equals(userPlan, StringComparison.OrdinalIgnoreCase)
        );

        var requiredPlanIndex = Array.FindIndex(
            PlanHierarchy,
            p => p.Equals(requiredPlan, StringComparison.OrdinalIgnoreCase)
        );

        // Unknown plans default to lowest tier
        if (userPlanIndex < 0)
            userPlanIndex = 0;
        if (requiredPlanIndex < 0)
            requiredPlanIndex = 0;

        return userPlanIndex >= requiredPlanIndex;
    }
}
