using FrogEdu.AI.Domain.Entities;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.AI.Domain.Interfaces;

/// <summary>
/// Repository interface for TutorSession aggregate
/// </summary>
public interface ITutorSessionRepository : IRepository<TutorSession>
{
    Task<IReadOnlyList<TutorSession>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task<TutorSession?> GetWithMessagesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<TutorSession>> GetActiveSessionsAsync(
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<TutorSession>> GetExpiredSessionsAsync(
        CancellationToken cancellationToken = default
    );
}
