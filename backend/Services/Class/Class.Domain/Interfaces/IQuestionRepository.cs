using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Interfaces;

/// <summary>
/// Repository interface for Question aggregate
/// </summary>
public interface IQuestionRepository : IRepository<Question>
{
    Task<IReadOnlyList<Question>> GetByTextbookIdAsync(
        Guid textbookId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetByChapterIdAsync(
        Guid chapterId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetByDifficultyAsync(
        Difficulty difficulty,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Question>> GetByTextbookAndDifficultyAsync(
        Guid textbookId,
        Difficulty difficulty,
        CancellationToken cancellationToken = default
    );
    Task<Question?> GetWithOptionsAsync(Guid id, CancellationToken cancellationToken = default);
}
