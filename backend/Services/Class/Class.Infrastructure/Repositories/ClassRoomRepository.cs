using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class ClassRoomRepository : IClassRoomRepository
{
    private readonly ClassDbContext _context;

    public ClassRoomRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<ClassRoom?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<ClassRoom?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .FirstOrDefaultAsync(c => c.InviteCode.Value == inviteCode, cancellationToken);
    }

    public async Task<IReadOnlyList<ClassRoom>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .Where(c => c.TeacherId == teacherId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassRoom>> GetActiveByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .Where(c => c.TeacherId == teacherId && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassRoom>> GetByGradeAsync(
        string grade,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .Where(c => c.Grade == grade && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassRoom>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassRoom>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        var enrolledClassIds = await _context
            .ClassEnrollments.Where(e =>
                e.StudentId == studentId && e.Status == Domain.Enums.EnrollmentStatus.Active
            )
            .Select(e => e.ClassId)
            .ToListAsync(cancellationToken);

        return await _context
            .ClassRooms.Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .Where(c => enrolledClassIds.Contains(c.Id))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ClassRoom classRoom, CancellationToken cancellationToken = default)
    {
        await _context.ClassRooms.AddAsync(classRoom, cancellationToken);
    }

    public void Update(ClassRoom classRoom)
    {
        _context.ClassRooms.Update(classRoom);
    }

    public void Delete(ClassRoom classRoom)
    {
        _context.ClassRooms.Remove(classRoom);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
