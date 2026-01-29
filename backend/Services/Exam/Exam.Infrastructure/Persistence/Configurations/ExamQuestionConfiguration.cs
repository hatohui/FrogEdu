using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.ToTable("ExamQuestions");

        builder.HasKey(eq => eq.Id);

        builder.Property(eq => eq.ExamId).IsRequired();

        builder.Property(eq => eq.QuestionId).IsRequired();

        builder.Property(eq => eq.OrderIndex).IsRequired();

        builder.HasIndex(eq => new { eq.ExamId, eq.QuestionId }).IsUnique();

        builder
            .HasOne(eq => eq.Question)
            .WithMany()
            .HasForeignKey(eq => eq.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
