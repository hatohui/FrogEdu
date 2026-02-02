using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Events;
using FrogEdu.Subscription.Domain.ValueObjects;

namespace FrogEdu.Subscription.Domain.Entities;

public sealed class Transaction : Entity
{
    public string TransactionCode { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;
    public PaymentProvider PaymentProvider { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public string? ProviderTransactionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserSubscriptionId { get; private set; }

    private Transaction() { }

    private Transaction(
        string transactionCode,
        Money amount,
        PaymentProvider paymentProvider,
        Guid userSubscriptionId
    )
    {
        TransactionCode = transactionCode;
        Amount = amount;
        PaymentProvider = paymentProvider;
        PaymentStatus = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UserSubscriptionId = userSubscriptionId;
    }

    public static Transaction Create(
        string transactionCode,
        Money amount,
        PaymentProvider paymentProvider,
        Guid userSubscriptionId
    )
    {
        if (string.IsNullOrWhiteSpace(transactionCode))
            throw new ArgumentException(
                "Transaction code cannot be empty",
                nameof(transactionCode)
            );
        if (userSubscriptionId == Guid.Empty)
            throw new ArgumentException(
                "User subscription ID cannot be empty",
                nameof(userSubscriptionId)
            );

        var transaction = new Transaction(
            transactionCode,
            amount,
            paymentProvider,
            userSubscriptionId
        );
        transaction.AddDomainEvent(
            new TransactionCreatedDomainEvent(transaction.Id, transactionCode, amount.Amount)
        );
        return transaction;
    }

    public void UpdateStatus(PaymentStatus status, string? providerTransactionId = null)
    {
        var previousStatus = PaymentStatus;
        PaymentStatus = status;
        ProviderTransactionId = providerTransactionId;

        if (status == PaymentStatus.Paid && previousStatus != PaymentStatus.Paid)
        {
            AddDomainEvent(new PaymentCompletedDomainEvent(Id, UserSubscriptionId, Amount.Amount));
        }
        else if (status == PaymentStatus.Failed)
        {
            AddDomainEvent(new PaymentFailedDomainEvent(Id, TransactionCode));
        }
    }

    public void MarkAsPaid(string providerTransactionId)
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new InvalidOperationException("Transaction is already marked as paid");

        UpdateStatus(PaymentStatus.Paid, providerTransactionId);
    }

    public void MarkAsFailed()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new InvalidOperationException("Cannot mark a paid transaction as failed");

        UpdateStatus(PaymentStatus.Failed);
    }

    public void MarkAsCancelled()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid transaction");

        UpdateStatus(PaymentStatus.Cancelled);
    }
}
