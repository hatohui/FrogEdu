using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Persistence;

/// <summary>
/// Database context for Class service
/// </summary>
public class ClassDbContext : DbContext
{
    public ClassDbContext(DbContextOptions<ClassDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity configurations will be added here
    }
}
