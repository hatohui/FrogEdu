namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO for renewing a subscription (Admin only)
/// </summary>
public sealed record RenewSubscriptionDto
{
    public DateTime NewEndDate { get; init; }
}
