using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.Events;

public sealed record SubscriptionCreatedDomainEvent(Guid SubscriptionId, Guid UserId, Guid TierId)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SubscriptionRenewedDomainEvent(
    Guid SubscriptionId,
    Guid UserId,
    DateTime NewEndDate
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SubscriptionCancelledDomainEvent(Guid SubscriptionId, Guid UserId)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SubscriptionExpiredDomainEvent(Guid SubscriptionId, Guid UserId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TransactionCreatedDomainEvent(
    Guid TransactionId,
    string TransactionCode,
    decimal Amount
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PaymentCompletedDomainEvent(
    Guid TransactionId,
    Guid SubscriptionId,
    decimal Amount
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PaymentFailedDomainEvent(Guid TransactionId, string TransactionCode)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
