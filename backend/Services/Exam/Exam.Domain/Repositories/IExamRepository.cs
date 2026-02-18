namespace FrogEdu.Exam.Domain.Repositories;

public interface IExamRepository
{
    Task<Entities.Exam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Exam>> GetByCreatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Entities.Exam>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Entities.Exam>> GetActiveExamsAsync(
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Entities.Exam>> GetDraftExamsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Entities.Exam exam, CancellationToken cancellationToken = default);
    void Update(Entities.Exam exam);
    void Delete(Entities.Exam exam);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
