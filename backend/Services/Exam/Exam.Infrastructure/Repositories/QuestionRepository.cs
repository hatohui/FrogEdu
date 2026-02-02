using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ExamDbContext _context;

    public QuestionRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Questions.Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> GetByTopicIdAsync(
        Guid topicId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Questions.Include(q => q.Answers)
            .Where(q => q.TopicId == topicId)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> GetByCognitiveLevelAsync(
        CognitiveLevel level,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Questions.Include(q => q.Answers)
            .Where(q => q.CognitiveLevel == level)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> GetPublicQuestionsAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Questions.Include(q => q.Answers)
            .Where(q => q.IsPublic)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> GetByCreatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Questions.Include(q => q.Answers)
            .Where(q => q.CreatedBy == userId)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Question question, CancellationToken cancellationToken = default)
    {
        await _context.Questions.AddAsync(question, cancellationToken);
    }

    public void Update(Question question)
    {
        _context.Questions.Update(question);
    }

    public void Delete(Question question)
    {
        _context.Questions.Remove(question);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
