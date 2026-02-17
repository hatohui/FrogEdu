using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;

namespace FrogEdu.Class.Domain.Repositories;

public interface IStudentExamAttemptRepository
{
    Task<StudentExamAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StudentExamAttempt?> GetByIdWithAnswersAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<StudentExamAttempt>> GetBySessionIdAsync(
        Guid examSessionId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<StudentExamAttempt>> GetByStudentAndSessionAsync(
        Guid studentId,
        Guid examSessionId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<StudentExamAttempt>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );

    Task<int> GetAttemptCountAsync(
        Guid studentId,
        Guid examSessionId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(StudentExamAttempt attempt, CancellationToken cancellationToken = default);
    void Update(StudentExamAttempt attempt);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
