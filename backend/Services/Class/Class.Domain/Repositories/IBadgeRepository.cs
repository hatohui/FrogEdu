using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Repositories;

public interface IBadgeRepository
{
    Task<Badge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Badge>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Badge>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Badge badge, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
