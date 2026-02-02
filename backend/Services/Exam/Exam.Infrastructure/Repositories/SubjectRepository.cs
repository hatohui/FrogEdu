using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly ExamDbContext _context;

    public SubjectRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<Subject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Subject?> GetByCodeAsync(
        string subjectCode,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Subjects.FirstOrDefaultAsync(
            s => s.SubjectCode == subjectCode,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Subject>> GetByGradeAsync(
        int grade,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Subjects.Where(s => s.Grade == grade)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Subject>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Subjects.OrderBy(s => s.Grade)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        await _context.Subjects.AddAsync(subject, cancellationToken);
    }

    public void Update(Subject subject)
    {
        _context.Subjects.Update(subject);
    }

    public void Delete(Subject subject)
    {
        _context.Subjects.Remove(subject);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
