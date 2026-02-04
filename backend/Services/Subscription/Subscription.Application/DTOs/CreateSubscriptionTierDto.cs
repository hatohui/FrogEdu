namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO for creating a new subscription tier (Admin only)
/// </summary>
public sealed record CreateSubscriptionTierDto
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "VND";
    public int DurationInDays { get; init; }
    public string TargetRole { get; init; } = null!;
    public string? ImageUrl { get; init; }
}
