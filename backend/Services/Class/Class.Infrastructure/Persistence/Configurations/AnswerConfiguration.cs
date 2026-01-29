using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.SubmissionId).IsRequired();
        builder.Property(a => a.QuestionId).IsRequired();
        builder.Property(a => a.SelectedOptionIds).HasColumnType("uuid[]");
        builder.Property(a => a.AnswerText).HasMaxLength(4000);
        builder.Property(a => a.Score).HasPrecision(5, 2);
        builder.Property(a => a.Feedback).HasMaxLength(2000);

        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.UpdatedAt).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(256);
        builder.Property(a => a.UpdatedBy).HasMaxLength(256);
        builder.Property(a => a.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(a => a.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(a => a.SubmissionId).HasDatabaseName("IX_Answers_SubmissionId");
        builder.HasIndex(a => a.QuestionId).HasDatabaseName("IX_Answers_QuestionId");
        builder
            .HasIndex(a => new { a.SubmissionId, a.QuestionId })
            .IsUnique()
            .HasDatabaseName("IX_Answers_SubmissionId_QuestionId");
        builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_Answers_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
