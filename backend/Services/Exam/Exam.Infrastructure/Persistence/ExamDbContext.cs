using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Persistence;

/// <summary>
/// Database context for Exam service
/// </summary>
public class ExamDbContext : DbContext
{
    public ExamDbContext(DbContextOptions<ExamDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity configurations will be added here
    }
}
