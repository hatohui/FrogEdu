using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.Entities;

/// <summary>
/// Transaction entity - represents a payment transaction
/// </summary>
public sealed class Transaction : Entity
{
    public Guid UserId { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public PaymentProvider Provider { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string? TransactionId { get; private set; }
    public string? PaymentUrl { get; private set; }
    public DateTime? PaidAt { get; private set; }

    // Navigation property
    public Subscription Subscription { get; private set; } = null!;

    private Transaction() { }

    private Transaction(
        Guid userId,
        Guid subscriptionId,
        decimal amount,
        string currency,
        PaymentProvider provider
    )
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        Amount = amount;
        Currency = currency;
        Provider = provider;
        Status = TransactionStatus.Pending;
    }

    public static Transaction Create(
        Guid userId,
        Guid subscriptionId,
        decimal amount,
        string currency,
        PaymentProvider provider
    )
    {
        return new Transaction(userId, subscriptionId, amount, currency, provider);
    }

    public void SetPaymentUrl(string paymentUrl)
    {
        PaymentUrl = paymentUrl;
        UpdateTimestamp();
    }

    public void MarkAsCompleted(string transactionId)
    {
        Status = TransactionStatus.Completed;
        TransactionId = transactionId;
        PaidAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void MarkAsFailed()
    {
        Status = TransactionStatus.Failed;
        UpdateTimestamp();
    }

    public void MarkAsCancelled()
    {
        Status = TransactionStatus.Cancelled;
        UpdateTimestamp();
    }
}

public enum TransactionStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4,
}
