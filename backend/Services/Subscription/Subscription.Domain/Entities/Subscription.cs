using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.Entities;

/// <summary>
/// Subscription aggregate root - represents a user's subscription plan
/// </summary>
public sealed class Subscription : Entity
{
    public Guid UserId { get; private set; }
    public PlanType PlanType { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public PaymentProvider Provider { get; private set; }
    public SubscriptionStatus Status { get; private set; }

    private Subscription() { }

    private Subscription(Guid userId, PlanType planType, PaymentProvider provider)
    {
        UserId = userId;
        PlanType = planType;
        Provider = provider;
        StartDate = DateTime.UtcNow;
        Status = SubscriptionStatus.Active;

        EndDate = planType == PlanType.Pro ? DateTime.UtcNow.AddDays(30) : null;
    }

    public static Subscription CreateFree(Guid userId)
    {
        return new Subscription(userId, PlanType.Free, PaymentProvider.None);
    }

    public static Subscription CreatePro(Guid userId, PaymentProvider provider)
    {
        return new Subscription(userId, PlanType.Pro, provider);
    }

    public void Renew(int days = 30)
    {
        if (PlanType == PlanType.Pro)
        {
            EndDate = (EndDate ?? DateTime.UtcNow).AddDays(days);
            Status = SubscriptionStatus.Active;
            UpdateTimestamp();
        }
    }

    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
        UpdateTimestamp();
    }

    public void Expire()
    {
        Status = SubscriptionStatus.Expired;
        UpdateTimestamp();
    }

    public bool IsActive() =>
        Status == SubscriptionStatus.Active && (EndDate == null || EndDate > DateTime.UtcNow);
}

public enum PlanType
{
    Free = 1,
    Pro = 2,
}

public enum PaymentProvider
{
    None = 0,
    VNPay = 1,
    Stripe = 2,
}

public enum SubscriptionStatus
{
    Active = 1,
    Cancelled = 2,
    Expired = 3,
}
