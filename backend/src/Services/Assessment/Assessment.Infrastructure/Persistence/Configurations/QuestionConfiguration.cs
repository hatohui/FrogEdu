using FrogEdu.Assessment.Domain.Entities;
using FrogEdu.Assessment.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Assessment.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Question aggregate
/// </summary>
public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id).ValueGeneratedNever();

        builder.Property(q => q.Content).IsRequired().HasMaxLength(4000);

        builder.Property(q => q.Type).IsRequired().HasConversion<int>();

        builder.Property(q => q.Difficulty).IsRequired().HasConversion<int>();

        builder.Property(q => q.Points).IsRequired().HasColumnType("decimal(5,2)");

        builder.Property(q => q.TextbookId).IsRequired();

        builder.Property(q => q.ChapterId);

        builder.Property(q => q.Explanation).HasMaxLength(2000);

        builder.Property(q => q.ImageS3Key).HasMaxLength(1000);

        builder.Property(q => q.LearningObjectives).HasMaxLength(1000);

        builder.Property(q => q.CreatedAt).IsRequired();

        builder.Property(q => q.UpdatedAt).IsRequired();

        builder.Property(q => q.CreatedBy).HasMaxLength(256);

        builder.Property(q => q.UpdatedBy).HasMaxLength(256);

        builder.Property(q => q.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(q => q.TextbookId).HasDatabaseName("IX_Questions_TextbookId");

        builder.HasIndex(q => q.ChapterId).HasDatabaseName("IX_Questions_ChapterId");

        builder.HasIndex(q => q.Difficulty).HasDatabaseName("IX_Questions_Difficulty");

        builder
            .HasIndex(q => new { q.TextbookId, q.Difficulty })
            .HasDatabaseName("IX_Questions_TextbookId_Difficulty");

        builder.HasIndex(q => q.Type).HasDatabaseName("IX_Questions_Type");

        builder.HasIndex(q => q.IsDeleted).HasDatabaseName("IX_Questions_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}
