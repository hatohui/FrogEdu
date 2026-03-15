using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class BadgeRepository : IBadgeRepository
{
    private readonly ClassDbContext _context;

    public BadgeRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<Badge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Badges.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Badge>> GetAllActiveAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Badges.Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Badge>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Badges.OrderBy(b => b.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Badge badge, CancellationToken cancellationToken = default)
    {
        await _context.Badges.AddAsync(badge, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
