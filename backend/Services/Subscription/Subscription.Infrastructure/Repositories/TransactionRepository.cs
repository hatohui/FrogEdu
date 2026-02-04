using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly SubscriptionDbContext _context;

    public TransactionRepository(SubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Transaction?> GetByTransactionCodeAsync(
        string transactionCode,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Transactions.FirstOrDefaultAsync(
            t => t.TransactionCode == transactionCode,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Transaction>> GetBySubscriptionIdAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Transactions.Where(t => t.UserSubscriptionId == subscriptionId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var subscriptionIds = await _context
            .UserSubscriptions.Where(s => s.UserId == userId)
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        return await _context
            .Transactions.Where(t => subscriptionIds.Contains(t.UserSubscriptionId))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByStatusAsync(
        PaymentStatus status,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Transactions.Where(t => t.PaymentStatus == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByProviderAsync(
        PaymentProvider provider,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Transactions.Where(t => t.PaymentProvider == provider)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Transactions.OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default
    )
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public void Update(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
