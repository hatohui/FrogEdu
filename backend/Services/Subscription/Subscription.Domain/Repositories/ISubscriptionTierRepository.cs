using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Repositories;

public interface ISubscriptionTierRepository
{
    Task<SubscriptionTier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SubscriptionTier?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<SubscriptionTier>> GetAllActiveAsync(
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<SubscriptionTier>> GetAllAsync(
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<SubscriptionTier>> GetByTargetRoleAsync(
        string targetRole,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(SubscriptionTier tier, CancellationToken cancellationToken = default);
    void Update(SubscriptionTier tier);
    void Delete(SubscriptionTier tier);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
