using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Interfaces;

public interface IClassRepository
{
    Task<Entities.Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Class?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Entities.Class>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<Entities.Class?> GetWithMembershipsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Entities.Class classEntity, CancellationToken cancellationToken = default);
    void Update(Entities.Class classEntity);
    void Remove(Entities.Class classEntity);
}
