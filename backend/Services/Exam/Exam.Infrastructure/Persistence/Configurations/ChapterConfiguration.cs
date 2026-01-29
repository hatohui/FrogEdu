using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Chapter entity
/// </summary>
public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("Chapters");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.ChapterNumber).IsRequired();

        builder.Property(c => c.Title).IsRequired().HasMaxLength(500);

        builder.Property(c => c.Description).HasMaxLength(2000);

        builder.Property(c => c.TextbookId).IsRequired();

        builder.Property(c => c.CreatedAt).IsRequired();

        builder.Property(c => c.UpdatedAt).IsRequired();

        builder.Property(c => c.CreatedBy).HasMaxLength(256);

        builder.Property(c => c.UpdatedBy).HasMaxLength(256);

        builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasOne(c => c.Textbook)
            .WithMany(t => t.Chapters)
            .HasForeignKey(c => c.TextbookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Pages)
            .WithOne(p => p.Chapter)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.TextbookId).HasDatabaseName("IX_Chapters_TextbookId");

        builder
            .HasIndex(c => new { c.TextbookId, c.ChapterNumber })
            .IsUnique()
            .HasDatabaseName("IX_Chapters_TextbookId_ChapterNumber");

        builder.HasIndex(c => c.IsDeleted).HasDatabaseName("IX_Chapters_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}


