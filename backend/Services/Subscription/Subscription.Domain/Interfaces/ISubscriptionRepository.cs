using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Subscription?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Subscription>> GetActiveSubscriptionsAsync(
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
    void Update(Subscription subscription);
    void Remove(Subscription subscription);
}
