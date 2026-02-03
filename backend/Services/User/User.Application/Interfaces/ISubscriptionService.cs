namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Service for fetching subscription information from the Subscription microservice
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// Get subscription claims for a user (plan, expiration, etc.)
    /// </summary>
    Task<SubscriptionClaimsDto> GetSubscriptionClaimsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}

/// <summary>
/// DTO for subscription claims
/// </summary>
public sealed record SubscriptionClaimsDto
{
    public Guid UserId { get; init; }
    public string Plan { get; init; } = "free";
    public long ExpiresAt { get; init; }
    public bool HasActiveSubscription { get; init; }
}
