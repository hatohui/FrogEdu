using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;

namespace FrogEdu.Class.Domain.Repositories;

public interface IClassEnrollmentRepository
{
    Task<ClassEnrollment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClassEnrollment?> GetByClassAndStudentAsync(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassEnrollment>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassEnrollment>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ClassEnrollment>> GetByStatusAsync(
        Guid classId,
        EnrollmentStatus status,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(ClassEnrollment enrollment, CancellationToken cancellationToken = default);
    void Update(ClassEnrollment enrollment);
    void Delete(ClassEnrollment enrollment);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
