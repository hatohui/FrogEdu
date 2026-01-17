using FrogEdu.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UserEntity = FrogEdu.User.Domain.Entities.User;

namespace FrogEdu.User.Infrastructure.Persistence;

/// <summary>
/// DbContext for User Service
/// Manages user profiles, authentication data, and class enrollment
/// Database: frog-user-db (PostgreSQL via Neon)
/// </summary>
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL specific settings
        modelBuilder.HasDefaultSchema("public");

        // User entity configuration
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity
                .Property(e => e.CognitoId)
                .HasConversion(
                    v => v.Value,
                    v => FrogEdu.User.Domain.ValueObjects.CognitoUserId.Create(v)
                )
                .IsRequired();
            entity
                .Property(e => e.Email)
                .HasConversion(v => v.Value, v => FrogEdu.User.Domain.ValueObjects.Email.Create(v))
                .IsRequired()
                .HasMaxLength(256);
            entity
                .Property(e => e.FullName)
                .HasConversion(
                    v => v.FirstName + " " + v.LastName,
                    v =>
                        FrogEdu.User.Domain.ValueObjects.FullName.Create(
                            v.Substring(0, v.IndexOf(' ') > 0 ? v.IndexOf(' ') : v.Length),
                            v.IndexOf(' ') > 0 ? v.Substring(v.IndexOf(' ') + 1) : ""
                        )
                )
                .IsRequired()
                .HasMaxLength(256);
            entity.HasIndex(e => e.CognitoId).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

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
}
