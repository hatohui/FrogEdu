using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Subject).HasMaxLength(50);
        builder.Property(c => c.Grade).IsRequired();
        builder.Property(c => c.HomeroomTeacherId).IsRequired();
        builder.Property(c => c.School).HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.MaxStudents);
        builder.Property(c => c.IsArchived).IsRequired().HasDefaultValue(false);

        // Configure InviteCode as owned entity
        builder.OwnsOne(
            c => c.InviteCode,
            inviteCode =>
            {
                inviteCode
                    .Property(i => i.Value)
                    .HasColumnName("InviteCode")
                    .HasMaxLength(6)
                    .IsFixedLength();

                inviteCode.Property(i => i.ExpiresAt).HasColumnName("InviteCodeExpiresAt");

                // Unique index on invite code
                inviteCode
                    .HasIndex(i => i.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Classes_InviteCode");
            }
        );

        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
        builder.Property(c => c.CreatedBy).HasMaxLength(256);
        builder.Property(c => c.UpdatedBy).HasMaxLength(256);
        builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(c => c.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(c => c.HomeroomTeacherId).HasDatabaseName("IX_Classes_HomeroomTeacherId");
        builder.HasIndex(c => c.Grade).HasDatabaseName("IX_Classes_Grade");
        builder.HasIndex(c => c.IsDeleted).HasDatabaseName("IX_Classes_IsDeleted");
        builder.HasIndex(c => c.IsArchived).HasDatabaseName("IX_Classes_IsArchived");

        // Note: Enrollment relationship managed via EnrollmentConfiguration

        // Global query filter for soft delete
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
