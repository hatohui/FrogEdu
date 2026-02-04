namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Constants for subscription-related claims and policies
/// </summary>
public static class SubscriptionConstants
{
    /// <summary>
    /// HttpContext item key for storing subscription claims
    /// </summary>
    public const string HttpContextKey = "SubscriptionClaims";

    /// <summary>
    /// Claim type for subscription plan
    /// </summary>
    public const string PlanClaimType = "subscription:plan";

    /// <summary>
    /// Claim type for subscription expiration
    /// </summary>
    public const string ExpiresAtClaimType = "subscription:expires_at";

    /// <summary>
    /// Claim type for active subscription status
    /// </summary>
    public const string HasActiveSubscriptionClaimType = "subscription:active";

    /// <summary>
    /// Available subscription plans
    /// </summary>
    public static class Plans
    {
        public const string Free = "free";
        public const string Pro = "pro";
        public const string Enterprise = "enterprise";
    }

    /// <summary>
    /// Authorization policy names
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// Requires any active paid subscription
        /// </summary>
        public const string RequireActiveSubscription = "RequireActiveSubscription";

        /// <summary>
        /// Requires Pro plan or higher
        /// </summary>
        public const string RequireProPlan = "RequireProPlan";

        /// <summary>
        /// Requires Enterprise plan
        /// </summary>
        public const string RequireEnterprisePlan = "RequireEnterprisePlan";
    }
}
