using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Repositories;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Assignment>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Assignment>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Assignment>> GetActiveAssignmentsAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Assignment>> GetOverdueAssignmentsAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Assignment assignment, CancellationToken cancellationToken = default);
    void Update(Assignment assignment);
    void Delete(Assignment assignment);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
