using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Content).IsRequired();

        builder.Property(q => q.Type).IsRequired().HasConversion<int>();

        builder.Property(q => q.Difficulty).IsRequired().HasConversion<int>();

        builder.Property(q => q.CorrectAnswer).HasMaxLength(500);

        builder.HasIndex(q => new { q.Difficulty, q.ChapterId });
    }
}
