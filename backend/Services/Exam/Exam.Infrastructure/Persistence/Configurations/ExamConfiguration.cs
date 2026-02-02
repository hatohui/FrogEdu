using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExamEntity = FrogEdu.Exam.Domain.Entities.Exam;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class ExamConfiguration : IEntityTypeConfiguration<ExamEntity>
{
    public void Configure(EntityTypeBuilder<ExamEntity> builder)
    {
        builder.ToTable("Exams");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Title).IsRequired().HasMaxLength(256);

        builder.Property(e => e.Duration).IsRequired();

        builder.Property(e => e.AccessCode).HasMaxLength(50);

        builder.Property(e => e.PassScore).IsRequired();

        builder.Property(e => e.MaxAttempts).IsRequired();

        builder.Property(e => e.StartTime).IsRequired();

        builder.Property(e => e.EndTime).IsRequired();

        builder.Property(e => e.ShouldShuffleQuestions).IsRequired().HasDefaultValue(false);

        builder.Property(e => e.ShouldShuffleAnswerOptions).IsRequired().HasDefaultValue(false);

        builder.Property(e => e.IsDraft).IsRequired().HasDefaultValue(true);

        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(false);

        builder.Property(e => e.TopicId).IsRequired();

        // Auditable properties
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.UpdatedBy).HasMaxLength(256);

        // Indexes
        builder
            .HasIndex(e => e.AccessCode)
            .IsUnique()
            .HasDatabaseName("IX_Exams_AccessCode")
            .HasFilter("[AccessCode] IS NOT NULL");

        builder.HasIndex(e => e.TopicId).HasDatabaseName("IX_Exams_TopicId");

        builder.HasIndex(e => e.IsDraft).HasDatabaseName("IX_Exams_IsDraft");

        builder.HasIndex(e => e.IsActive).HasDatabaseName("IX_Exams_IsActive");

        builder.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Exams_CreatedBy");
    }
}
