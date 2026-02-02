using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.Subscription.Domain.ValueObjects;

namespace FrogEdu.Subscription.Domain.Entities;

public sealed class SubscriptionTier : Entity
{
    public string Name { get; private set; } = null!;
    public string? ImageUrl { get; private set; }
    public string Description { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public int DurationInDays { get; private set; }
    public string TargetRole { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private SubscriptionTier() { }

    private SubscriptionTier(
        string name,
        string description,
        Money price,
        int durationInDays,
        string targetRole,
        string? imageUrl = null
    )
    {
        Name = name;
        Description = description;
        Price = price;
        DurationInDays = durationInDays;
        TargetRole = targetRole;
        ImageUrl = imageUrl;
        IsActive = true;
    }

    public static SubscriptionTier Create(
        string name,
        string description,
        Money price,
        int durationInDays,
        string targetRole,
        string? imageUrl = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (durationInDays <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(durationInDays));
        if (string.IsNullOrWhiteSpace(targetRole))
            throw new ArgumentException("Target role cannot be empty", nameof(targetRole));

        return new SubscriptionTier(name, description, price, durationInDays, targetRole, imageUrl);
    }

    public void Update(
        string name,
        string description,
        Money price,
        int durationInDays,
        string? imageUrl = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (durationInDays <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(durationInDays));

        Name = name;
        Description = description;
        Price = price;
        DurationInDays = durationInDays;
        ImageUrl = imageUrl;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
