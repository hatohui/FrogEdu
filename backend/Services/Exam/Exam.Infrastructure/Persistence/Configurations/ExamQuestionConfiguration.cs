using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.ToTable("ExamQuestions");

        // Composite primary key
        builder.HasKey(eq => new { eq.ExamId, eq.QuestionId });

        builder.Property(eq => eq.ExamId).IsRequired();
        builder.Property(eq => eq.QuestionId).IsRequired();

        // Indexes
        builder.HasIndex(eq => eq.ExamId).HasDatabaseName("IX_ExamQuestions_ExamId");

        builder.HasIndex(eq => eq.QuestionId).HasDatabaseName("IX_ExamQuestions_QuestionId");

        // Relationships
        builder
            .HasOne<Domain.Entities.Exam>()
            .WithMany(e => e.ExamQuestions)
            .HasForeignKey(eq => eq.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Question>()
            .WithMany()
            .HasForeignKey(eq => eq.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
