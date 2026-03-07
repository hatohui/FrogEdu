using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Repositories;

public class AIUsageRecordRepository : IAIUsageRecordRepository
{
    private readonly SubscriptionDbContext _context;

    public AIUsageRecordRepository(SubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetUsageCountAsync(Guid userId, string? actionType = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AIUsageRecords.Where(r => r.UserId == userId);
        if (!string.IsNullOrWhiteSpace(actionType))
            query = query.Where(r => r.ActionType == actionType);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> GetUsageCountSinceAsync(Guid userId, DateTime since, string? actionType = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AIUsageRecords.Where(r => r.UserId == userId && r.UsedAt >= since);
        if (!string.IsNullOrWhiteSpace(actionType))
            query = query.Where(r => r.ActionType == actionType);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AIUsageRecord>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AIUsageRecords
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.UsedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AIUsageRecord>> GetByUserIdSinceAsync(Guid userId, DateTime since, CancellationToken cancellationToken = default)
    {
        return await _context.AIUsageRecords
            .Where(r => r.UserId == userId && r.UsedAt >= since)
            .OrderByDescending(r => r.UsedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AIUsageRecord record, CancellationToken cancellationToken = default)
    {
        await _context.AIUsageRecords.AddAsync(record, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
