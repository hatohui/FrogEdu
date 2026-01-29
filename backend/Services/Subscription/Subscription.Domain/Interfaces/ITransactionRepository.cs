using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<Transaction?> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    void Update(Transaction transaction);
}
