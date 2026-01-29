using System.Linq.Expressions;
using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Exam.Domain.Interfaces;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Textbook aggregate
/// </summary>
public class TextbookRepository : ITextbookRepository
{
    private readonly ExamDbContext _context;

    public TextbookRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<Textbook?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Textbooks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Textbook>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.OrderBy(t => t.GradeLevel)
            .ThenBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Textbook>> FindAsync(
        Expression<Func<Textbook, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Textbooks.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Textbook> AddAsync(
        Textbook entity,
        CancellationToken cancellationToken = default
    )
    {
        await _context.Textbooks.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Textbook entity, CancellationToken cancellationToken = default)
    {
        _context.Textbooks.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Textbook entity, CancellationToken cancellationToken = default)
    {
        entity.MarkAsDeleted();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Textbooks.AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Textbook>> GetByGradeLevelAsync(
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.Where(t => t.GradeLevel == gradeLevel)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Textbook>> GetBySubjectAsync(
        string subject,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.Where(t => EF.Property<string>(t.Subject, "Name") == subject)
            .OrderBy(t => t.GradeLevel)
            .ThenBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Textbook>> GetByGradeAndSubjectAsync(
        GradeLevel gradeLevel,
        string subject,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.Where(t =>
                t.GradeLevel == gradeLevel && EF.Property<string>(t.Subject, "Name") == subject
            )
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Textbook?> GetWithChaptersAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.Include(t => t.Chapters)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Textbook?> GetWithChaptersAndPagesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Textbooks.Include(t => t.Chapters)
                .ThenInclude(c => c.Pages)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}


