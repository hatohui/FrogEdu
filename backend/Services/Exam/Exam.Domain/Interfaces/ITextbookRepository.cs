using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Interfaces;

/// <summary>
/// Repository interface for Textbook aggregate
/// </summary>
public interface ITextbookRepository : IRepository<Textbook>
{
    Task<IReadOnlyList<Textbook>> GetByGradeLevelAsync(
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Textbook>> GetBySubjectAsync(
        string subject,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Textbook>> GetByGradeAndSubjectAsync(
        GradeLevel gradeLevel,
        string subject,
        CancellationToken cancellationToken = default
    );
    Task<Textbook?> GetWithChaptersAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Textbook?> GetWithChaptersAndPagesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}


