using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Interfaces;

public interface IExamRepository
{
    Task<Entities.Exam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Exam?> GetWithQuestionsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Entities.Exam>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Entities.Exam>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Entities.Exam exam, CancellationToken cancellationToken = default);
    void Update(Entities.Exam exam);
    void Remove(Entities.Exam exam);
}
