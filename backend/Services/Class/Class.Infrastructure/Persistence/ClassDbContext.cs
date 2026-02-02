using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Persistence;

public class ClassDbContext : DbContext
{
    public ClassDbContext(DbContextOptions<ClassDbContext> options)
        : base(options) { }

    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<ClassEnrollment> ClassEnrollments => Set<ClassEnrollment>();
    public DbSet<Assignment> Assignments => Set<Assignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfiguration(new ClassRoomConfiguration());
        modelBuilder.ApplyConfiguration(new ClassEnrollmentConfiguration());
        modelBuilder.ApplyConfiguration(new AssignmentConfiguration());

        // Auto-set timestamps for auditable entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdAtProp = entityType.FindProperty("CreatedAt");
            if (createdAtProp?.ClrType == typeof(DateTime))
            {
                createdAtProp.SetDefaultValueSql("CURRENT_TIMESTAMP");
            }

            var updatedAtProp = entityType.FindProperty("UpdatedAt");
            if (updatedAtProp?.ClrType == typeof(DateTime?))
            {
                updatedAtProp.SetDefaultValue(null);
            }
        }
    }
}
