using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topics");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(256);

        builder.Property(t => t.Description).IsRequired().HasMaxLength(2000);

        builder.Property(t => t.IsCurriculum).IsRequired().HasDefaultValue(false);

        builder.Property(t => t.SubjectId).IsRequired();

        // Auditable properties
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.CreatedBy).IsRequired(false);
        builder.Property(t => t.UpdatedAt).IsRequired(false);
        builder.Property(t => t.UpdatedBy).IsRequired(false);

        // Indexes
        builder.HasIndex(t => t.SubjectId).HasDatabaseName("IX_Topics_SubjectId");

        builder.HasIndex(t => t.IsCurriculum).HasDatabaseName("IX_Topics_IsCurriculum");

        // Relationships
        builder
            .HasMany(t => t.Questions)
            .WithOne()
            .HasForeignKey(q => q.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
