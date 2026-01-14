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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new TextbookConfiguration());
        modelBuilder.ApplyConfiguration(new ChapterConfiguration());
        modelBuilder.ApplyConfiguration(new PageConfiguration());

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
