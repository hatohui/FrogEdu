using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Textbook aggregate
/// </summary>
public class TextbookConfiguration : IEntityTypeConfiguration<Textbook>
{
    public void Configure(EntityTypeBuilder<Textbook> builder)
    {
        builder.ToTable("Textbooks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(500);

        builder.Property(t => t.GradeLevel).IsRequired().HasConversion<int>();

        // Configure Subject as owned type (stored as string in database)
        builder.OwnsOne(
            t => t.Subject,
            subject =>
            {
                subject
                    .Property(s => s.Name)
                    .HasColumnName("Subject")
                    .IsRequired()
                    .HasMaxLength(100);
            }
        );

        builder.Property(t => t.Publisher).HasMaxLength(300);

        builder.Property(t => t.PublicationYear);

        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.Property(t => t.CoverImageS3Key).HasMaxLength(500);

        builder.Property(t => t.CreatedAt).IsRequired();

        builder.Property(t => t.UpdatedAt).IsRequired();

        builder.Property(t => t.CreatedBy).HasMaxLength(256);

        builder.Property(t => t.UpdatedBy).HasMaxLength(256);

        builder.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasMany(t => t.Chapters)
            .WithOne(c => c.Textbook)
            .HasForeignKey(c => c.TextbookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(t => t.GradeLevel).HasDatabaseName("IX_Textbooks_GradeLevel");

        builder
            .HasIndex(t => new { t.GradeLevel })
            .IncludeProperties(t => new { t.Title, t.CreatedAt })
            .HasDatabaseName("IX_Textbooks_GradeLevel_Subject");

        builder.HasIndex(t => t.IsDeleted).HasDatabaseName("IX_Textbooks_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}


