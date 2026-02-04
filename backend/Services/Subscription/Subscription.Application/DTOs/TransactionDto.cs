namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO representing a transaction record
/// </summary>
public sealed record TransactionDto
{
    public Guid Id { get; init; }
    public string TransactionCode { get; init; } = null!;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    public string PaymentProvider { get; init; } = null!;
    public string PaymentStatus { get; init; } = null!;
    public string? ProviderTransactionId { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UserSubscriptionId { get; init; }
    public string? SubscriptionPlanName { get; init; }
}
