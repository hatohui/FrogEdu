using FrogEdu.Class.Domain.Entities;

namespace FrogEdu.Class.Domain.Interfaces;

public interface IClassMembershipRepository
{
    Task<ClassMembership?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassMembership>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<ClassMembership>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task<ClassMembership?> GetByClassAndStudentAsync(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(ClassMembership membership, CancellationToken cancellationToken = default);
    void Remove(ClassMembership membership);
}
