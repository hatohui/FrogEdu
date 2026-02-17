using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class StudentExamAttemptConfiguration : IEntityTypeConfiguration<StudentExamAttempt>
{
    public void Configure(EntityTypeBuilder<StudentExamAttempt> builder)
    {
        builder.ToTable("StudentExamAttempts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.ExamSessionId).IsRequired();
        builder.Property(a => a.StudentId).IsRequired();
        builder.Property(a => a.StartedAt).IsRequired();
        builder.Property(a => a.SubmittedAt);

        builder.Property(a => a.Score).IsRequired().HasPrecision(10, 2).HasDefaultValue(0);

        builder.Property(a => a.TotalPoints).IsRequired().HasPrecision(10, 2).HasDefaultValue(0);

        builder.Property(a => a.AttemptNumber).IsRequired().HasDefaultValue(1);

        builder.Property(a => a.Status).IsRequired().HasConversion<string>().HasMaxLength(20);

        // Relationships
        builder
            .HasMany(a => a.Answers)
            .WithOne()
            .HasForeignKey(sa => sa.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder
            .HasIndex(a => a.ExamSessionId)
            .HasDatabaseName("IX_StudentExamAttempts_ExamSessionId");
        builder.HasIndex(a => a.StudentId).HasDatabaseName("IX_StudentExamAttempts_StudentId");
        builder
            .HasIndex(a => new { a.ExamSessionId, a.StudentId })
            .HasDatabaseName("IX_StudentExamAttempts_SessionId_StudentId");
        builder.HasIndex(a => a.Status).HasDatabaseName("IX_StudentExamAttempts_Status");
    }
}
