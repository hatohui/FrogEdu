using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Repositories;

public interface IAIUsageRecordRepository
{
    Task<IReadOnlyList<AIUsageRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> GetUsageCountAsync(
        Guid userId,
        string? actionType = null,
        CancellationToken cancellationToken = default
    );
    Task<int> GetUsageCountSinceAsync(
        Guid userId,
        DateTime since,
        string? actionType = null,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<AIUsageRecord>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<AIUsageRecord>> GetByUserIdSinceAsync(
        Guid userId,
        DateTime since,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(AIUsageRecord record, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
