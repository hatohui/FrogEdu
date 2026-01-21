using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.User.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IClassRepository
/// </summary>
public sealed class ClassRepository : IClassRepository
{
    private readonly UserDbContext _context;

    public ClassRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Classes.Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Class?> GetByIdWithEnrollmentsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Classes.Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Class?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    )
    {
        var normalizedCode = inviteCode.ToUpperInvariant();
        return await _context
            .Classes.Include(c => c.Enrollments)
            .FirstOrDefaultAsync(
                c => c.InviteCode != null && c.InviteCode.Value == normalizedCode,
                cancellationToken
            );
    }

    public async Task<IReadOnlyList<Class>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Classes.Include(c => c.Enrollments)
            .Where(c => c.HomeroomTeacherId == teacherId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Class>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Classes.Include(c => c.Enrollments)
            .Where(c =>
                c.Enrollments.Any(e => e.UserId == studentId && e.Role == EnrollmentRole.Student)
            )
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> InviteCodeExistsAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    )
    {
        var normalizedCode = inviteCode.ToUpperInvariant();
        return await _context.Classes.AnyAsync(
            c => c.InviteCode != null && c.InviteCode.Value == normalizedCode,
            cancellationToken
        );
    }

    public async Task AddAsync(Class classEntity, CancellationToken cancellationToken = default)
    {
        await _context.Classes.AddAsync(classEntity, cancellationToken);
    }

    public void Update(Class classEntity)
    {
        _context.Classes.Update(classEntity);
    }

    public void Delete(Class classEntity)
    {
        // Soft delete - will be handled by entity method
        _context.Classes.Update(classEntity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
