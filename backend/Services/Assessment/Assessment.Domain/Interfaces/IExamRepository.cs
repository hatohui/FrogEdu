using FrogEdu.Assessment.Domain.Entities;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.Assessment.Domain.Interfaces;

/// <summary>
/// Repository interface for ExamPaper aggregate
/// </summary>
public interface IExamRepository : IRepository<ExamPaper>
{
    Task<IReadOnlyList<ExamPaper>> GetByTextbookIdAsync(
        Guid textbookId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<ExamPaper>> GetByCreatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<ExamPaper?> GetWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExamPaper?> GetWithQuestionsAndOptionsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}
