namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO representing a subscription tier/plan
/// </summary>
public sealed record SubscriptionTierDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? ImageUrl { get; init; }
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "VND";
    public int DurationInDays { get; init; }
    public string TargetRole { get; init; } = null!;
    public bool IsActive { get; init; }
}
