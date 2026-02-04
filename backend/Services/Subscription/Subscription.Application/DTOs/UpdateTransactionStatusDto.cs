namespace FrogEdu.Subscription.Application.DTOs;

/// <summary>
/// DTO for updating transaction payment status (Admin only)
/// </summary>
public sealed record UpdateTransactionStatusDto
{
    public string PaymentStatus { get; init; } = null!;
    public string? ProviderTransactionId { get; init; }
}
