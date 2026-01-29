using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Interfaces;

public interface IClassRepository
{
    Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Class?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Class>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<Class?> GetWithMembershipsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Class classEntity, CancellationToken cancellationToken = default);
    void Update(Class classEntity);
    void Remove(Class classEntity);
}
