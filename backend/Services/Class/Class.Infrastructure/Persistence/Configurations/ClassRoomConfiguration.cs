using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        builder.ToTable("ClassRooms");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(256);

        builder.Property(c => c.Grade).IsRequired().HasMaxLength(10);

        // Convert InviteCode value object
        builder
            .Property(c => c.InviteCode)
            .HasConversion(v => v.Value, v => InviteCode.Create(v))
            .IsRequired()
            .HasMaxLength(6)
            .HasColumnName("InviteCode");

        builder.Property(c => c.MaxStudents).IsRequired();

        builder.Property(c => c.BannerUrl).HasMaxLength(1000);

        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(c => c.TeacherId).IsRequired();

        // Auditable properties
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.UpdatedBy).HasMaxLength(256);

        // Indexes
        builder.HasIndex(c => c.InviteCode).IsUnique().HasDatabaseName("IX_ClassRooms_InviteCode");

        builder.HasIndex(c => c.TeacherId).HasDatabaseName("IX_ClassRooms_TeacherId");

        builder.HasIndex(c => c.Grade).HasDatabaseName("IX_ClassRooms_Grade");

        builder.HasIndex(c => c.IsActive).HasDatabaseName("IX_ClassRooms_IsActive");

        // Relationships
        builder
            .HasMany(c => c.Enrollments)
            .WithOne()
            .HasForeignKey(e => e.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Assignments)
            .WithOne()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
