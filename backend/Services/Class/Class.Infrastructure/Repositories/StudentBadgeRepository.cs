using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Repositories;

public class StudentBadgeRepository : IStudentBadgeRepository
{
    private readonly ClassDbContext _context;

    public StudentBadgeRepository(ClassDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<StudentBadge>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentBadges.Where(sb => sb.StudentId == studentId)
            .OrderByDescending(sb => sb.AwardedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentBadge>> GetByClassIdAsync(
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentBadges.Where(sb => sb.ClassId == classId)
            .OrderByDescending(sb => sb.AwardedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentBadge>> GetByStudentAndClassAsync(
        Guid studentId,
        Guid classId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .StudentBadges.Where(sb => sb.StudentId == studentId && sb.ClassId == classId)
            .OrderByDescending(sb => sb.AwardedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        StudentBadge studentBadge,
        CancellationToken cancellationToken = default
    )
    {
        await _context.StudentBadges.AddAsync(studentBadge, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
