using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Question>> GetByChapterIdAsync(
        Guid chapterId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Question>> GetByDifficultyAsync(
        DifficultyLevel difficulty,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Question>> GetRandomByDifficultyAndChapterAsync(
        DifficultyLevel difficulty,
        Guid? chapterId,
        int count,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Question question, CancellationToken cancellationToken = default);
    void Update(Question question);
    void Remove(Question question);
}
