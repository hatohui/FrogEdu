using Microsoft.AspNetCore.Http;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Extension methods for accessing subscription claims from HttpContext
/// </summary>
public static class SubscriptionClaimsExtensions
{
    /// <summary>
    /// Get subscription claims from HttpContext.
    /// Returns null if claims are not available (middleware not run or user not authenticated).
    /// </summary>
    public static SubscriptionClaimsDto? GetSubscriptionClaims(this HttpContext context)
    {
        if (context.Items.TryGetValue(SubscriptionConstants.HttpContextKey, out var claims))
        {
            return claims as SubscriptionClaimsDto;
        }

        return null;
    }

    /// <summary>
    /// Set subscription claims in HttpContext.
    /// </summary>
    public static void SetSubscriptionClaims(this HttpContext context, SubscriptionClaimsDto claims)
    {
        context.Items[SubscriptionConstants.HttpContextKey] = claims;
    }

    /// <summary>
    /// Check if user has an active subscription.
    /// Returns false if claims are not available.
    /// </summary>
    public static bool HasActiveSubscription(this HttpContext context)
    {
        var claims = context.GetSubscriptionClaims();
        return claims?.HasActiveSubscription ?? false;
    }

    /// <summary>
    /// Check if user has a specific subscription plan.
    /// Returns false if claims are not available.
    /// </summary>
    public static bool HasSubscriptionPlan(this HttpContext context, string plan)
    {
        var claims = context.GetSubscriptionClaims();
        return claims?.HasPlan(plan) ?? false;
    }

    /// <summary>
    /// Check if user is a paid subscriber (non-free plan with active subscription).
    /// Returns false if claims are not available.
    /// </summary>
    public static bool IsPaidSubscriber(this HttpContext context)
    {
        var claims = context.GetSubscriptionClaims();
        return claims?.IsPaidSubscriber ?? false;
    }

    /// <summary>
    /// Get the user's current subscription plan.
    /// Returns "free" if claims are not available.
    /// </summary>
    public static string GetSubscriptionPlan(this HttpContext context)
    {
        var claims = context.GetSubscriptionClaims();
        return claims?.Plan ?? SubscriptionConstants.Plans.Free;
    }
}
