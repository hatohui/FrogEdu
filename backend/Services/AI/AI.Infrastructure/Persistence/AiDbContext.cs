using FrogEdu.AI.Domain.Entities;
using FrogEdu.AI.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.AI.Infrastructure.Persistence;

/// <summary>
/// DbContext for AI Orchestrator Service
/// </summary>
public class AiDbContext : DbContext
{
    public AiDbContext(DbContextOptions<AiDbContext> options)
        : base(options) { }

    public DbSet<TutorSession> TutorSessions => Set<TutorSession>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();
    public DbSet<PromptTemplate> PromptTemplates => Set<PromptTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL specific settings
        modelBuilder.HasDefaultSchema("public");

        // Apply configurations
        modelBuilder.ApplyConfiguration(new TutorSessionConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationMessageConfiguration());
        modelBuilder.ApplyConfiguration(new PromptTemplateConfiguration());

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
