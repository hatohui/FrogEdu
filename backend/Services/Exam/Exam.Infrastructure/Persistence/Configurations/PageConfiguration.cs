using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Page entity
/// </summary>
public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        builder.ToTable("Pages");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.PageNumber).IsRequired();

        builder.Property(p => p.S3Key).IsRequired().HasMaxLength(1000);

        builder.Property(p => p.Description).HasMaxLength(1000);

        builder.Property(p => p.ChapterId).IsRequired();

        builder.Property(p => p.LessonId); // Optional - pages can belong to lessons

        builder.Property(p => p.CreatedAt).IsRequired();

        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.Property(p => p.CreatedBy).HasMaxLength(256);

        builder.Property(p => p.UpdatedBy).HasMaxLength(256);

        builder.Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.Property(p => p.RowVersion).IsRowVersion();

        // Configure relationships
        builder
            .HasOne(p => p.Chapter)
            .WithMany(c => c.Pages)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(p => p.ChapterId).HasDatabaseName("IX_Pages_ChapterId");

        builder.HasIndex(p => p.LessonId).HasDatabaseName("IX_Pages_LessonId");

        builder
            .HasIndex(p => new { p.ChapterId, p.PageNumber })
            .IsUnique()
            .HasDatabaseName("IX_Pages_ChapterId_PageNumber");

        builder.HasIndex(p => p.IsDeleted).HasDatabaseName("IX_Pages_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}


