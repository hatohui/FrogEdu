using FrogEdu.Assessment.Domain.Entities;
using FrogEdu.Assessment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Assessment.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ExamPaper aggregate
/// </summary>
public class ExamPaperConfiguration : IEntityTypeConfiguration<ExamPaper>
{
    public void Configure(EntityTypeBuilder<ExamPaper> builder)
    {
        builder.ToTable("ExamPapers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Title).IsRequired().HasMaxLength(500);

        // Configure ExamMatrix as owned type (stored as separate columns)
        builder.OwnsOne(
            e => e.Matrix,
            matrix =>
            {
                matrix.Property(m => m.EasyCount).HasColumnName("MatrixEasyCount").IsRequired();

                matrix.Property(m => m.MediumCount).HasColumnName("MatrixMediumCount").IsRequired();

                matrix.Property(m => m.HardCount).HasColumnName("MatrixHardCount").IsRequired();

                matrix
                    .Property(m => m.EasyPoints)
                    .HasColumnName("MatrixEasyPoints")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                matrix
                    .Property(m => m.MediumPoints)
                    .HasColumnName("MatrixMediumPoints")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                matrix
                    .Property(m => m.HardPoints)
                    .HasColumnName("MatrixHardPoints")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();
            }
        );

        builder.Property(e => e.TextbookId).IsRequired();

        builder.Property(e => e.CreatedByUserId).IsRequired();

        builder.Property(e => e.DurationMinutes).IsRequired();

        builder.Property(e => e.Instructions).HasMaxLength(2000);

        builder.Property(e => e.ExamPdfS3Key).HasMaxLength(1000);

        builder.Property(e => e.AnswerKeyPdfS3Key).HasMaxLength(1000);

        builder.Property(e => e.Version).IsRequired().HasDefaultValue(1);

        builder.Property(e => e.CreatedAt).IsRequired();

        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.Property(e => e.CreatedBy).HasMaxLength(256);

        builder.Property(e => e.UpdatedBy).HasMaxLength(256);

        builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasMany(e => e.ExamQuestions)
            .WithOne(eq => eq.ExamPaper)
            .HasForeignKey(eq => eq.ExamPaperId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.TextbookId).HasDatabaseName("IX_ExamPapers_TextbookId");

        builder.HasIndex(e => e.CreatedByUserId).HasDatabaseName("IX_ExamPapers_CreatedByUserId");

        builder.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_ExamPapers_CreatedAt");

        builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_ExamPapers_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
