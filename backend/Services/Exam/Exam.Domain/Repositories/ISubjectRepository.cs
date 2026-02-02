using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Repositories;

public interface ISubjectRepository
{
    Task<Subject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Subject?> GetByCodeAsync(
        string subjectCode,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Subject>> GetByGradeAsync(
        int grade,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Subject subject, CancellationToken cancellationToken = default);
    void Update(Subject subject);
    void Delete(Subject subject);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
