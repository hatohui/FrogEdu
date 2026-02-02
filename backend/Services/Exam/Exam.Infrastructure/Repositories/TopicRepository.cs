using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly ExamDbContext _context;

    public TopicRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Topics.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Topic>> GetBySubjectIdAsync(
        Guid subjectId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Topics.Where(t => t.SubjectId == subjectId)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Topic>> GetCurriculumTopicsAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Topics.Where(t => t.IsCurriculum)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        await _context.Topics.AddAsync(topic, cancellationToken);
    }

    public void Update(Topic topic)
    {
        _context.Topics.Update(topic);
    }

    public void Delete(Topic topic)
    {
        _context.Topics.Remove(topic);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
