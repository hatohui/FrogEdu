using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly SubscriptionDbContext _context;

    public UserSubscriptionRepository(SubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task<UserSubscription?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .FirstOrDefaultAsync(us => us.Id == id, cancellationToken);
    }

    public async Task<UserSubscription?> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .Where(us => us.UserId == userId && us.Status == SubscriptionStatus.Active)
            .OrderByDescending(us => us.EndDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserSubscription>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .Where(us => us.UserId == userId)
            .OrderByDescending(us => us.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserSubscription>> GetByStatusAsync(
        SubscriptionStatus status,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .Where(us => us.Status == status)
            .OrderByDescending(us => us.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserSubscription>> GetExpiringSubscriptionsAsync(
        DateTime beforeDate,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .Where(us => us.Status == SubscriptionStatus.Active && us.EndDate <= beforeDate)
            .OrderBy(us => us.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserSubscription>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .UserSubscriptions.Include(us => us.Transactions)
            .OrderByDescending(us => us.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        UserSubscription subscription,
        CancellationToken cancellationToken = default
    )
    {
        await _context.UserSubscriptions.AddAsync(subscription, cancellationToken);
    }

    public void Update(UserSubscription subscription)
    {
        _context.UserSubscriptions.Update(subscription);
    }

    public void Delete(UserSubscription subscription)
    {
        _context.UserSubscriptions.Remove(subscription);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
