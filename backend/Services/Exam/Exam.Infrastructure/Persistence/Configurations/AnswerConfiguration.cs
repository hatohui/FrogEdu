using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Content).IsRequired().HasMaxLength(2000);

        builder.Property(a => a.IsCorrect).IsRequired().HasDefaultValue(false);

        builder.Property(a => a.Explanation).HasMaxLength(2000);

        builder.Property(a => a.QuestionId).IsRequired();

        // Indexes
        builder.HasIndex(a => a.QuestionId).HasDatabaseName("IX_Answers_QuestionId");

        builder.HasIndex(a => a.IsCorrect).HasDatabaseName("IX_Answers_IsCorrect");
    }
}
