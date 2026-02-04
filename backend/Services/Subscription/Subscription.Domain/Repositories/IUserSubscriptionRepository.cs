using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Enums;

namespace FrogEdu.Subscription.Domain.Repositories;

public interface IUserSubscriptionRepository
{
    Task<UserSubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserSubscription?> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<UserSubscription>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<UserSubscription>> GetByStatusAsync(
        SubscriptionStatus status,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<UserSubscription>> GetExpiringSubscriptionsAsync(
        DateTime beforeDate,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<UserSubscription>> GetAllAsync(
        CancellationToken cancellationToken = default
    );
    Task AddAsync(UserSubscription subscription, CancellationToken cancellationToken = default);
    void Update(UserSubscription subscription);
    void Delete(UserSubscription subscription);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
