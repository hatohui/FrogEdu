using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class ClassEnrollmentRepository : IClassEnrollmentRepository
{
    private readonly ClassDbContext _context;

    public ClassEnrollmentRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<ClassEnrollment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.ClassEnrollments.FirstOrDefaultAsync(
            ce => ce.Id == id,
            cancellationToken
        );
    }

    public async Task<ClassEnrollment?> GetByClassAndStudentAsync(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.ClassEnrollments.FirstOrDefaultAsync(
            ce => ce.ClassId == classId && ce.StudentId == studentId,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<ClassEnrollment>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassEnrollments.Where(ce => ce.ClassId == classId)
            .OrderByDescending(ce => ce.JoinedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassEnrollment>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassEnrollments.Where(ce => ce.StudentId == studentId)
            .OrderByDescending(ce => ce.JoinedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ClassEnrollment>> GetByStatusAsync(
        Guid classId,
        EnrollmentStatus status,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ClassEnrollments.Where(ce => ce.ClassId == classId && ce.Status == status)
            .OrderByDescending(ce => ce.JoinedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        ClassEnrollment enrollment,
        CancellationToken cancellationToken = default
    )
    {
        await _context.ClassEnrollments.AddAsync(enrollment, cancellationToken);
    }

    public void Update(ClassEnrollment enrollment)
    {
        _context.ClassEnrollments.Update(enrollment);
    }

    public void Delete(ClassEnrollment enrollment)
    {
        _context.ClassEnrollments.Remove(enrollment);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
