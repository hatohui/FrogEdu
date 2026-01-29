using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Persistence;

/// <summary>
/// DbContext for Exam Service
/// </summary>
public class ExamDbContext : DbContext
{
    public ExamDbContext(DbContextOptions<ExamDbContext> options)
        : base(options) { }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Entities.Exam> Exams => Set<Entities.Exam>();
    public DbSet<ExamQuestion> ExamQuestions => Set<ExamQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL specific settings
        modelBuilder.HasDefaultSchema("public");

        // Apply configurations
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new ExamConfiguration());
        modelBuilder.ApplyConfiguration(new ExamQuestionConfiguration());

        // TODO: Add Textbook-related configurations once Domain entities are created
        // modelBuilder.ApplyConfiguration(new TextbookConfiguration());
        // modelBuilder.ApplyConfiguration(new ChapterConfiguration());
        // modelBuilder.ApplyConfiguration(new PageConfiguration());
        // modelBuilder.ApplyConfiguration(new LessonConfiguration());
        // modelBuilder.ApplyConfiguration(new TagConfiguration());
        // modelBuilder.ApplyConfiguration(new LessonTagConfiguration());
        // modelBuilder.ApplyConfiguration(new AssetConfiguration());

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
                // CreatedAt is set in constructor, but ensure UpdatedAt is also set
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
