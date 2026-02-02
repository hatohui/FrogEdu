using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Repositories;

public interface ITopicRepository
{
    Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Topic>> GetBySubjectIdAsync(
        Guid subjectId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Topic>> GetCurriculumTopicsAsync(
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Topic topic, CancellationToken cancellationToken = default);
    void Update(Topic topic);
    void Delete(Topic topic);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
