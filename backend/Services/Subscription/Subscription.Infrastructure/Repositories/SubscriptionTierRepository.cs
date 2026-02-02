using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Repositories;

public class SubscriptionTierRepository : ISubscriptionTierRepository
{
    private readonly SubscriptionDbContext _context;

    public SubscriptionTierRepository(SubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionTier?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.SubscriptionTiers.FirstOrDefaultAsync(
            st => st.Id == id,
            cancellationToken
        );
    }

    public async Task<SubscriptionTier?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.SubscriptionTiers.FirstOrDefaultAsync(
            st => st.Name == name,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<SubscriptionTier>> GetAllActiveAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .SubscriptionTiers.Where(st => st.IsActive)
            .OrderBy(st => st.DurationInDays)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SubscriptionTier>> GetByTargetRoleAsync(
        string targetRole,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .SubscriptionTiers.Where(st => st.TargetRole == targetRole && st.IsActive)
            .OrderBy(st => st.DurationInDays)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(SubscriptionTier tier, CancellationToken cancellationToken = default)
    {
        await _context.SubscriptionTiers.AddAsync(tier, cancellationToken);
    }

    public void Update(SubscriptionTier tier)
    {
        _context.SubscriptionTiers.Update(tier);
    }

    public void Delete(SubscriptionTier tier)
    {
        _context.SubscriptionTiers.Remove(tier);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
