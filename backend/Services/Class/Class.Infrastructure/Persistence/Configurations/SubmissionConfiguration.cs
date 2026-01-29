using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.ToTable("Submissions");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.ExamId).IsRequired();
        builder.Property(s => s.StudentId).IsRequired();
        builder.Property(s => s.StartedAt).IsRequired();
        builder.Property(s => s.SubmittedAt);
        builder.Property(s => s.Score).HasPrecision(5, 2);
        builder.Property(s => s.Status).IsRequired().HasConversion<int>();

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
        builder.Property(s => s.CreatedBy).HasMaxLength(256);
        builder.Property(s => s.UpdatedBy).HasMaxLength(256);
        builder.Property(s => s.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(s => s.ExamId).HasDatabaseName("IX_Submissions_ExamId");
        builder.HasIndex(s => s.StudentId).HasDatabaseName("IX_Submissions_StudentId");
        builder
            .HasIndex(s => new { s.ExamId, s.StudentId })
            .HasDatabaseName("IX_Submissions_ExamId_StudentId");
        builder.HasIndex(s => s.Status).HasDatabaseName("IX_Submissions_Status");
        builder.HasIndex(s => s.IsDeleted).HasDatabaseName("IX_Submissions_IsDeleted");

        // Note: Removed _answers relationship - Answers managed separately

        // Global query filter for soft delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
