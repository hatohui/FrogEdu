using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ExamSessionConfiguration : IEntityTypeConfiguration<ExamSession>
{
    public void Configure(EntityTypeBuilder<ExamSession> builder)
    {
        builder.ToTable("ExamSessions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.ClassId).IsRequired();
        builder.Property(e => e.ExamId).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EndTime).IsRequired();
        builder.Property(e => e.RetryTimes).IsRequired().HasDefaultValue(1);
        builder.Property(e => e.IsRetryable).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.ShouldShuffleQuestions).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.ShouldShuffleAnswers).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.AllowPartialScoring).IsRequired().HasDefaultValue(true);

        // Auditing
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CreatedBy);
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.UpdatedBy);

        // Relationships
        builder
            .HasMany(e => e.Attempts)
            .WithOne()
            .HasForeignKey(a => a.ExamSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.ClassId).HasDatabaseName("IX_ExamSessions_ClassId");
        builder.HasIndex(e => e.ExamId).HasDatabaseName("IX_ExamSessions_ExamId");
        builder.HasIndex(e => e.StartTime).HasDatabaseName("IX_ExamSessions_StartTime");
        builder.HasIndex(e => e.EndTime).HasDatabaseName("IX_ExamSessions_EndTime");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("IX_ExamSessions_IsActive");
        builder
            .HasIndex(e => new { e.ClassId, e.ExamId })
            .HasDatabaseName("IX_ExamSessions_ClassId_ExamId");
    }
}
