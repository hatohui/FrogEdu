using FrogEdu.Content.Domain.Entities;
using FrogEdu.Content.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Content.Infrastructure.Persistence;

/// <summary>
/// DbContext for Content Service
/// </summary>
public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options)
        : base(options) { }

    public DbSet<Textbook> Textbooks => Set<Textbook>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Page> Pages => Set<Page>();

    // Note: Subject is a value object owned by Textbook, not a separate entity
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL specific settings
        modelBuilder.HasDefaultSchema("public");

        // Apply configurations
        modelBuilder.ApplyConfiguration(new TextbookConfiguration());
        modelBuilder.ApplyConfiguration(new ChapterConfiguration());
        modelBuilder.ApplyConfiguration(new PageConfiguration());
        // SubjectConfiguration removed - Subject is a value object
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new LessonTagConfiguration());
        modelBuilder.ApplyConfiguration(new AssetConfiguration());

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
