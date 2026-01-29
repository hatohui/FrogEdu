using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ExamQuestion entity
/// </summary>
public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.ToTable("ExamQuestions");

        builder.HasKey(eq => eq.Id);

        builder.Property(eq => eq.Id).ValueGeneratedNever();

        builder.Property(eq => eq.ExamPaperId).IsRequired();

        builder.Property(eq => eq.QuestionId).IsRequired();

        builder.Property(eq => eq.OrderIndex).IsRequired();

        builder.Property(eq => eq.Points).IsRequired().HasColumnType("decimal(5,2)");

        builder.Property(eq => eq.CreatedAt).IsRequired();

        builder.Property(eq => eq.UpdatedAt).IsRequired();

        builder.Property(eq => eq.CreatedBy).HasMaxLength(256);

        builder.Property(eq => eq.UpdatedBy).HasMaxLength(256);

        builder.Property(eq => eq.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasOne(eq => eq.ExamPaper)
            .WithMany(e => e.ExamQuestions)
            .HasForeignKey(eq => eq.ExamPaperId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(eq => eq.Question)
            .WithMany()
            .HasForeignKey(eq => eq.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(eq => eq.ExamPaperId).HasDatabaseName("IX_ExamQuestions_ExamPaperId");

        builder.HasIndex(eq => eq.QuestionId).HasDatabaseName("IX_ExamQuestions_QuestionId");

        builder
            .HasIndex(eq => new { eq.ExamPaperId, eq.QuestionId })
            .IsUnique()
            .HasDatabaseName("IX_ExamQuestions_ExamPaperId_QuestionId");

        builder
            .HasIndex(eq => new { eq.ExamPaperId, eq.OrderIndex })
            .IsUnique()
            .HasDatabaseName("IX_ExamQuestions_ExamPaperId_OrderIndex");

        builder.HasIndex(eq => eq.IsDeleted).HasDatabaseName("IX_ExamQuestions_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(eq => !eq.IsDeleted);
    }
}
