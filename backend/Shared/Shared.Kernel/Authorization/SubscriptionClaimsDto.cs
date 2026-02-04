namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// DTO representing subscription claims for a user.
/// This is used for inter-service communication and authorization decisions.
/// </summary>
public sealed record SubscriptionClaimsDto
{
    /// <summary>
    /// The user's unique identifier
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The subscription plan name (e.g., "free", "pro", "enterprise")
    /// </summary>
    public string Plan { get; init; } = "free";

    /// <summary>
    /// Unix timestamp when the subscription expires (0 for no subscription)
    /// </summary>
    public long ExpiresAt { get; init; }

    /// <summary>
    /// Whether the user has an active, non-expired subscription
    /// </summary>
    public bool HasActiveSubscription { get; init; }

    /// <summary>
    /// Check if the subscription is currently expired
    /// </summary>
    public bool IsExpired =>
        ExpiresAt > 0 && DateTimeOffset.FromUnixTimeSeconds(ExpiresAt) < DateTimeOffset.UtcNow;

    /// <summary>
    /// Check if user has a specific plan (case-insensitive)
    /// </summary>
    public bool HasPlan(string planName) =>
        string.Equals(Plan, planName, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Check if user has any of the specified plans (case-insensitive)
    /// </summary>
    public bool HasAnyPlan(params string[] planNames) =>
        planNames.Any(p => string.Equals(Plan, p, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Check if user has a paid subscription (not free)
    /// </summary>
    public bool IsPaidSubscriber =>
        HasActiveSubscription && !string.Equals(Plan, "free", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Creates a default free plan claims object
    /// </summary>
    public static SubscriptionClaimsDto FreePlan(Guid userId) =>
        new()
        {
            UserId = userId,
            Plan = "free",
            ExpiresAt = 0,
            HasActiveSubscription = false,
        };
}
