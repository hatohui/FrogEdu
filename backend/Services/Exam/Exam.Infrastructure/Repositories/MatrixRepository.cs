using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Repositories;

public class MatrixRepository : IMatrixRepository
{
    private readonly ExamDbContext _context;

    public MatrixRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<Matrix?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Matrices.Include(m => m.MatrixTopics)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Matrix?> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Matrices.Include(m => m.MatrixTopics)
            .FirstOrDefaultAsync(m => m.ExamId == examId, cancellationToken);
    }

    public async Task AddAsync(Matrix matrix, CancellationToken cancellationToken = default)
    {
        await _context.Matrices.AddAsync(matrix, cancellationToken);
    }

    public void Update(Matrix matrix)
    {
        _context.Matrices.Update(matrix);
    }

    public void Delete(Matrix matrix)
    {
        _context.Matrices.Remove(matrix);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
