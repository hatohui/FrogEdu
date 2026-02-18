using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ExamEntity = FrogEdu.Exam.Domain.Entities.Exam;

namespace FrogEdu.Exam.Infrastructure.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly ExamDbContext _context;

    public ExamRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<ExamEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Exams.Include(e => e.ExamQuestions)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExamEntity>> GetByCreatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Exams.Include(e => e.ExamQuestions)
            .Where(e => e.CreatedBy == userId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamEntity>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        var idList = ids.ToList();
        return await _context
            .Exams.Where(e => idList.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamEntity>> GetActiveExamsAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Exams.Include(e => e.ExamQuestions)
            .Where(e => e.IsActive && !e.IsDraft)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamEntity>> GetDraftExamsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Exams.Include(e => e.ExamQuestions)
            .Where(e => e.IsDraft && e.CreatedBy == userId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExamEntity exam, CancellationToken cancellationToken = default)
    {
        await _context.Exams.AddAsync(exam, cancellationToken);
    }

    public void Update(ExamEntity exam)
    {
        _context.Exams.Update(exam);
    }

    public void Delete(ExamEntity exam)
    {
        _context.Exams.Remove(exam);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
