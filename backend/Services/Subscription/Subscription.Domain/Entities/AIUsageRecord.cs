using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Subscription.Domain.Entities;

public sealed class AIUsageRecord : Entity
{
    public Guid UserId { get; private set; }
    public string ActionType { get; private set; } = null!;
    public DateTime UsedAt { get; private set; }
    public string? Metadata { get; private set; }

    private AIUsageRecord() { }

    private AIUsageRecord(Guid userId, string actionType, string? metadata = null)
    {
        UserId = userId;
        ActionType = actionType;
        UsedAt = DateTime.UtcNow;
        Metadata = metadata;
    }

    public static AIUsageRecord Create(Guid userId, string actionType, string? metadata = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        if (string.IsNullOrWhiteSpace(actionType))
            throw new ArgumentException("Action type cannot be empty", nameof(actionType));

        return new AIUsageRecord(userId, actionType, metadata);
    }
}
