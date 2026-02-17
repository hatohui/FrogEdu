using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Repositories;

public interface IExamSessionRepository
{
    Task<ExamSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExamSession?> GetByIdWithAttemptsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ExamSession>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ExamSession>> GetActiveByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ExamSession>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get all active exam sessions for a student across all enrolled classes
    /// </summary>
    Task<IReadOnlyList<ExamSession>> GetActiveSessionsForStudentAsync(
        IEnumerable<Guid> classIds,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get upcoming exam sessions for a student across all enrolled classes
    /// </summary>
    Task<IReadOnlyList<ExamSession>> GetUpcomingSessionsForStudentAsync(
        IEnumerable<Guid> classIds,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(ExamSession session, CancellationToken cancellationToken = default);
    void Update(ExamSession session);
    void Delete(ExamSession session);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
