using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class StudentExamAttemptRepository : IStudentExamAttemptRepository
{
    private readonly ClassDbContext _context;

    public StudentExamAttemptRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<StudentExamAttempt?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.StudentExamAttempts.FirstOrDefaultAsync(
            a => a.Id == id,
            cancellationToken
        );
    }

    public async Task<StudentExamAttempt?> GetByIdWithAnswersAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentExamAttempts.Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentExamAttempt>> GetBySessionIdAsync(
        Guid examSessionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentExamAttempts.Where(a => a.ExamSessionId == examSessionId)
            .OrderBy(a => a.StudentId)
            .ThenBy(a => a.AttemptNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentExamAttempt>> GetByStudentAndSessionAsync(
        Guid studentId,
        Guid examSessionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentExamAttempts.Where(a =>
                a.StudentId == studentId && a.ExamSessionId == examSessionId
            )
            .OrderBy(a => a.AttemptNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentExamAttempt>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentExamAttempts.Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.StartedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetAttemptCountAsync(
        Guid studentId,
        Guid examSessionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.StudentExamAttempts.CountAsync(
            a => a.StudentId == studentId && a.ExamSessionId == examSessionId,
            cancellationToken
        );
    }

    public async Task<Dictionary<Guid, int>> GetAttemptCountsForStudentAsync(
        Guid studentId,
        IEnumerable<Guid> examSessionIds,
        CancellationToken cancellationToken = default
    )
    {
        var idList = examSessionIds.ToList();
        return await _context
            .StudentExamAttempts.Where(a =>
                a.StudentId == studentId && idList.Contains(a.ExamSessionId)
            )
            .GroupBy(a => a.ExamSessionId)
            .Select(g => new { SessionId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SessionId, x => x.Count, cancellationToken);
    }

    public async Task AddAsync(
        StudentExamAttempt attempt,
        CancellationToken cancellationToken = default
    )
    {
        await _context.StudentExamAttempts.AddAsync(attempt, cancellationToken);
    }

    public void Update(StudentExamAttempt attempt)
    {
        _context.StudentExamAttempts.Update(attempt);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
