using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();

        builder.Property(l => l.ChapterId).IsRequired();
        builder.Property(l => l.OrderIndex).IsRequired();
        builder.Property(l => l.Title).IsRequired().HasMaxLength(500);
        builder.Property(l => l.Summary).HasMaxLength(2000);
        builder.Property(l => l.DurationMinutes);
        builder.Property(l => l.LearningObjectives).HasMaxLength(4000);

        builder.Property(l => l.CreatedAt).IsRequired();
        builder.Property(l => l.UpdatedAt).IsRequired();
        builder.Property(l => l.CreatedBy).HasMaxLength(256);
        builder.Property(l => l.UpdatedBy).HasMaxLength(256);
        builder.Property(l => l.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(l => l.RowVersion).IsRowVersion();

        // Indexes
        builder
            .HasIndex(l => new { l.ChapterId, l.OrderIndex })
            .IsUnique()
            .HasDatabaseName("IX_Lessons_ChapterId_OrderIndex");
        builder.HasIndex(l => l.IsDeleted).HasDatabaseName("IX_Lessons_IsDeleted");

        // Note: Removed _pages relationship - Pages belong to Chapters in current model
        // Page has optional LessonId but navigates via Chapter primarily

        // Many-to-many with Tags
        builder.HasMany(l => l.Tags).WithOne(lt => lt.Lesson).HasForeignKey(lt => lt.LessonId);

        // Global query filter for soft delete
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}


