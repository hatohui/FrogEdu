using Microsoft.AspNetCore.Authorization;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Authorization attribute that requires an active subscription.
/// Use this on controllers or actions that require any paid plan.
/// </summary>
/// <example>
/// [RequireActiveSubscription]
/// public async Task<IActionResult> PremiumFeature() { }
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireActiveSubscriptionAttribute : AuthorizeAttribute
{
    public RequireActiveSubscriptionAttribute()
    {
        Policy = SubscriptionConstants.Policies.RequireActiveSubscription;
    }
}

/// <summary>
/// Authorization attribute that requires a specific subscription plan.
/// Use this on controllers or actions that require a specific plan level.
/// </summary>
/// <example>
/// [RequirePlan("pro")]
/// public async Task<IActionResult> ProFeature() { }
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequirePlanAttribute : AuthorizeAttribute
{
    public string Plan { get; }

    public RequirePlanAttribute(string plan)
    {
        Plan = plan;
        Policy = $"RequirePlan:{plan}";
    }
}

/// <summary>
/// Authorization attribute that requires Pro plan or higher.
/// </summary>
/// <example>
/// [RequireProPlan]
/// public async Task<IActionResult> ProOnlyFeature() { }
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireProPlanAttribute : AuthorizeAttribute
{
    public RequireProPlanAttribute()
    {
        Policy = SubscriptionConstants.Policies.RequireProPlan;
    }
}

/// <summary>
/// Authorization attribute that requires Enterprise plan.
/// </summary>
/// <example>
/// [RequireEnterprisePlan]
/// public async Task<IActionResult> EnterpriseOnlyFeature() { }
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireEnterprisePlanAttribute : AuthorizeAttribute
{
    public RequireEnterprisePlanAttribute()
    {
        Policy = SubscriptionConstants.Policies.RequireEnterprisePlan;
    }
}
