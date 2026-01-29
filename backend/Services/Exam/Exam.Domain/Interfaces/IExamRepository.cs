using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Interfaces;

public interface IExamRepository
{
    Task<Exam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Exam?> GetWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Exam>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Exam>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Exam exam, CancellationToken cancellationToken = default);
    void Update(Exam exam);
    void Remove(Exam exam);
}
