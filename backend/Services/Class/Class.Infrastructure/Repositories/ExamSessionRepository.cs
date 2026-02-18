using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class ExamSessionRepository : IExamSessionRepository
{
    private readonly ClassDbContext _context;

    public ExamSessionRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<ExamSession?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.ExamSessions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<ExamSession?> GetByIdWithAttemptsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ExamSessions.Include(e => e.Attempts)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ExamSessions.Where(e => e.ClassId == classId)
            .OrderByDescending(e => e.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetActiveByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        return await _context
            .ExamSessions.Where(e =>
                e.ClassId == classId && e.IsActive && e.StartTime <= now && e.EndTime >= now
            )
            .OrderBy(e => e.EndTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ExamSessions.Where(e => e.ExamId == examId)
            .OrderByDescending(e => e.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetActiveSessionsForStudentAsync(
        IEnumerable<Guid> classIds,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        var classIdList = classIds.ToList();

        return await _context
            .ExamSessions.Where(e =>
                classIdList.Contains(e.ClassId)
                && e.IsActive
                && e.StartTime <= now
                && e.EndTime >= now
            )
            .OrderBy(e => e.EndTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetUpcomingSessionsForStudentAsync(
        IEnumerable<Guid> classIds,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        var classIdList = classIds.ToList();

        return await _context
            .ExamSessions.Where(e =>
                classIdList.Contains(e.ClassId) && e.IsActive && e.StartTime > now
            )
            .OrderBy(e => e.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExamSession>> GetAllSessionsForStudentAsync(
        IEnumerable<Guid> classIds,
        CancellationToken cancellationToken = default
    )
    {
        var classIdList = classIds.ToList();

        return await _context
            .ExamSessions.Where(e => classIdList.Contains(e.ClassId))
            .OrderByDescending(e => e.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExamSession session, CancellationToken cancellationToken = default)
    {
        await _context.ExamSessions.AddAsync(session, cancellationToken);
    }

    public void Update(ExamSession session)
    {
        _context.ExamSessions.Update(session);
    }

    public void Delete(ExamSession session)
    {
        _context.ExamSessions.Remove(session);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
