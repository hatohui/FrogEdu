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

        builder.Property(e => e.Name).IsRequired().HasMaxLength(256);

        builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);

        builder.Property(e => e.TopicId).IsRequired();

        builder.Property(e => e.SubjectId).IsRequired();

        builder.Property(e => e.Grade).IsRequired();

        builder.Property(e => e.IsDraft).IsRequired().HasDefaultValue(true);

        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(false);

        // Auditable properties
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.UpdatedBy).HasMaxLength(256);

        // Indexes
        builder.HasIndex(e => e.TopicId).HasDatabaseName("IX_Exams_TopicId");

        builder.HasIndex(e => e.SubjectId).HasDatabaseName("IX_Exams_SubjectId");

        builder.HasIndex(e => e.Grade).HasDatabaseName("IX_Exams_Grade");

        builder.HasIndex(e => e.IsDraft).HasDatabaseName("IX_Exams_IsDraft");

        builder.HasIndex(e => e.IsActive).HasDatabaseName("IX_Exams_IsActive");

        builder.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Exams_CreatedBy");
    }
}
