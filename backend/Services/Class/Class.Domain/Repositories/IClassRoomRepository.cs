using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Repositories;

public interface IClassRoomRepository
{
    Task<ClassRoom?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClassRoom?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassRoom>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassRoom>> GetActiveByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassRoom>> GetByGradeAsync(
        string grade,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(ClassRoom classRoom, CancellationToken cancellationToken = default);
    void Update(ClassRoom classRoom);
    void Delete(ClassRoom classRoom);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
