using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Domain.Repositories;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Question>> GetByTopicIdAsync(
        Guid topicId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetByCognitiveLevelAsync(
        CognitiveLevel level,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetPublicQuestionsAsync(
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetByCreatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetExamQuestionsAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Question question, CancellationToken cancellationToken = default);
    void Update(Question question);
    void Delete(Question question);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
