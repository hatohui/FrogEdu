using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Entities;

namespace FrogEdu.Subscription.Domain.Interfaces;

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
