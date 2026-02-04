using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Enums;

namespace FrogEdu.Subscription.Domain.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByTransactionCodeAsync(
        string transactionCode,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Transaction>> GetBySubscriptionIdAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Transaction>> GetByStatusAsync(
        PaymentStatus status,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Transaction>> GetByProviderAsync(
        PaymentProvider provider,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    void Update(Transaction transaction);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
