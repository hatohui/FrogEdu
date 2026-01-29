using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Persistence;

/// <summary>
/// DbContext for Class Service
/// </summary>
public class ClassDbContext : DbContext
{
    public ClassDbContext(DbContextOptions<ClassDbContext> options)
        : base(options) { }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
    public DbSet<ExamPaper> ExamPapers => Set<ExamPaper>();
    public DbSet<ExamQuestion> ExamQuestions => Set<ExamQuestion>();
    public DbSet<QuestionBank> QuestionBanks => Set<QuestionBank>();
    public DbSet<ExamSection> ExamSections => Set<ExamSection>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Rubric> Rubrics => Set<Rubric>();
    public DbSet<ExamGeneration> ExamGenerations => Set<ExamGeneration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL specific settings
        modelBuilder.HasDefaultSchema("public");

        // Apply configurations
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionOptionConfiguration());
        modelBuilder.ApplyConfiguration(new ExamPaperConfiguration());
        modelBuilder.ApplyConfiguration(new ExamQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionBankConfiguration());
        modelBuilder.ApplyConfiguration(new ExamSectionConfiguration());
        modelBuilder.ApplyConfiguration(new SubmissionConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new RubricConfiguration());
        modelBuilder.ApplyConfiguration(new ExamGenerationConfiguration());

        // Set default values for timestamp columns
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdAtProp = entityType.FindProperty("CreatedAt");
            if (createdAtProp?.ClrType == typeof(DateTime))
            {
                createdAtProp.SetDefaultValueSql("CURRENT_TIMESTAMP");
            }

            var updatedAtProp = entityType.FindProperty("UpdatedAt");
            if (updatedAtProp?.ClrType == typeof(DateTime))
            {
                updatedAtProp.SetDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps before saving
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is FrogEdu.Shared.Kernel.Entity
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        foreach (var entry in entries)
        {
            var entity = (FrogEdu.Shared.Kernel.Entity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.UpdateTimestamp();
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdateTimestamp();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
