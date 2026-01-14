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

        // Apply configurations
        modelBuilder.ApplyConfiguration(new TutorSessionConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationMessageConfiguration());
        modelBuilder.ApplyConfiguration(new PromptTemplateConfiguration());

        // Seed data (optional - will be added later)
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
