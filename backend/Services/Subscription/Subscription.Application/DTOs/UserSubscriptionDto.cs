namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO representing user subscription information
/// </summary>
public sealed record UserSubscriptionDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid SubscriptionTierId { get; init; }
    public string PlanName { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = null!;
    public bool IsActive { get; init; }
    public bool IsExpired { get; init; }
    public long ExpiresAtTimestamp { get; init; }
}

/// <summary>
/// Lightweight DTO for JWT claims - used by User Service
/// </summary>
public sealed record SubscriptionClaimsDto
{
    public Guid UserId { get; init; }
    public string Plan { get; init; } = "free";
    public long ExpiresAt { get; init; }
    public bool HasActiveSubscription { get; init; }
}
