using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Entities.Subscription?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task<Entities.Subscription?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Entities.Subscription>> GetActiveSubscriptionsAsync(
        CancellationToken cancellationToken = default
    );
    Task AddAsync(
        Entities.Subscription subscription,
        CancellationToken cancellationToken = default
    );
    void Update(Entities.Subscription subscription);
    void Remove(Entities.Subscription subscription);
}
