using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Events;

namespace FrogEdu.Subscription.Domain.Entities;

public sealed class UserSubscription : Entity
{
    public Guid UserId { get; private set; }
    public Guid SubscriptionTierId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private UserSubscription() { }

    private UserSubscription(
        Guid userId,
        Guid subscriptionTierId,
        DateTime startDate,
        DateTime endDate
    )
    {
        UserId = userId;
        SubscriptionTierId = subscriptionTierId;
        StartDate = startDate;
        EndDate = endDate;
        Status = SubscriptionStatus.Active;
    }

    public static UserSubscription Create(
        Guid userId,
        Guid subscriptionTierId,
        DateTime startDate,
        DateTime endDate
    )
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        if (subscriptionTierId == Guid.Empty)
            throw new ArgumentException(
                "Subscription tier ID cannot be empty",
                nameof(subscriptionTierId)
            );
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date", nameof(startDate));

        var subscription = new UserSubscription(userId, subscriptionTierId, startDate, endDate);
        subscription.AddDomainEvent(
            new SubscriptionCreatedDomainEvent(subscription.Id, userId, subscriptionTierId)
        );
        return subscription;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > EndDate;
    }

    public bool IsActive()
    {
        return Status == SubscriptionStatus.Active && !IsExpired();
    }

    public void Renew(DateTime newEndDate)
    {
        if (newEndDate <= EndDate)
            throw new ArgumentException(
                "New end date must be after current end date",
                nameof(newEndDate)
            );

        EndDate = newEndDate;
        Status = SubscriptionStatus.Active;
        AddDomainEvent(new SubscriptionRenewedDomainEvent(Id, UserId, newEndDate));
    }

    public void Cancel()
    {
        if (Status == SubscriptionStatus.Cancelled)
            throw new InvalidOperationException("Subscription is already cancelled");

        Status = SubscriptionStatus.Cancelled;
        AddDomainEvent(new SubscriptionCancelledDomainEvent(Id, UserId));
    }

    public void Suspend()
    {
        Status = SubscriptionStatus.Suspended;
    }

    public void Activate()
    {
        if (IsExpired())
            throw new InvalidOperationException("Cannot activate an expired subscription");

        Status = SubscriptionStatus.Active;
    }

    public void MarkAsExpired()
    {
        Status = SubscriptionStatus.Expired;
        AddDomainEvent(new SubscriptionExpiredDomainEvent(Id, UserId));
    }

    public void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }
}
