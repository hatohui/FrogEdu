using FrogEdu.Exam.Domain.Entities;

namespace FrogEdu.Exam.Domain.Repositories;

public interface IMatrixRepository
{
    Task<Matrix?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Matrix?> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken = default);
    Task AddAsync(Matrix matrix, CancellationToken cancellationToken = default);
    void Update(Matrix matrix);
    void Delete(Matrix matrix);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
