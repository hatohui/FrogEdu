using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
{
    public void Configure(EntityTypeBuilder<StudentAnswer> builder)
    {
        builder.ToTable("StudentAnswers");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.AttemptId).IsRequired();
        builder.Property(a => a.QuestionId).IsRequired();

        builder.Property(a => a.SelectedAnswerIds).IsRequired().HasMaxLength(2000);

        builder.Property(a => a.Score).IsRequired().HasPrecision(10, 2).HasDefaultValue(0);

        builder.Property(a => a.IsCorrect).IsRequired().HasDefaultValue(false);
        builder.Property(a => a.IsPartiallyCorrect).IsRequired().HasDefaultValue(false);

        // Indexes
        builder.HasIndex(a => a.AttemptId).HasDatabaseName("IX_StudentAnswers_AttemptId");
        builder
            .HasIndex(a => new { a.AttemptId, a.QuestionId })
            .IsUnique()
            .HasDatabaseName("IX_StudentAnswers_AttemptId_QuestionId");
    }
}
