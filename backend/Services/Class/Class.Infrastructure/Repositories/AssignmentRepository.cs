using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ClassDbContext _context;

    public AssignmentRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<Assignment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Assignment>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Assignments.Where(a => a.ClassId == classId)
            .OrderBy(a => a.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Assignment>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Assignments.Where(a => a.ExamId == examId)
            .OrderBy(a => a.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Assignment>> GetActiveAssignmentsAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        return await _context
            .Assignments.Where(a => a.ClassId == classId && a.StartDate <= now && a.DueDate >= now)
            .OrderBy(a => a.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Assignment>> GetOverdueAssignmentsAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        return await _context
            .Assignments.Where(a => a.ClassId == classId && a.DueDate < now)
            .OrderByDescending(a => a.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Assignment assignment, CancellationToken cancellationToken = default)
    {
        await _context.Assignments.AddAsync(assignment, cancellationToken);
    }

    public void Update(Assignment assignment)
    {
        _context.Assignments.Update(assignment);
    }

    public void Delete(Assignment assignment)
    {
        _context.Assignments.Remove(assignment);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
