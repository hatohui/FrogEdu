using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Repositories;

public interface IStudentBadgeRepository
{
    Task<IReadOnlyList<StudentBadge>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<StudentBadge>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<StudentBadge>> GetByStudentAndClassAsync(
        Guid studentId,
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(StudentBadge studentBadge, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
