using FrogEdu.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.ClassId).IsRequired();
        builder.Property(e => e.Role).IsRequired().HasConversion<int>();
        builder.Property(e => e.EnrolledAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.Property(e => e.CreatedBy).HasMaxLength(256);
        builder.Property(e => e.UpdatedBy).HasMaxLength(256);
        builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(e => e.UserId).HasDatabaseName("IX_Enrollments_UserId");
        builder.HasIndex(e => e.ClassId).HasDatabaseName("IX_Enrollments_ClassId");
        builder
            .HasIndex(e => new { e.UserId, e.ClassId })
            .IsUnique()
            .HasDatabaseName("IX_Enrollments_UserId_ClassId");
        builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Enrollments_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
