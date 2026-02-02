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
        builder.Property(q => q.Id).ValueGeneratedNever();

        builder.Property(q => q.Content).IsRequired().HasMaxLength(5000);

        builder.Property(q => q.Point).IsRequired().HasPrecision(5, 2);

        builder.Property(q => q.Type).IsRequired().HasConversion<string>().HasMaxLength(50);

        builder.Property(q => q.MediaUrl).HasMaxLength(1000);

        builder
            .Property(q => q.CognitiveLevel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(q => q.IsPublic).IsRequired().HasDefaultValue(false);

        builder.Property(q => q.Source).IsRequired().HasConversion<string>().HasMaxLength(50);

        builder.Property(q => q.TopicId).IsRequired();

        // Auditable properties
        builder.Property(q => q.CreatedAt).IsRequired();
        builder.Property(q => q.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(q => q.UpdatedAt);
        builder.Property(q => q.UpdatedBy).HasMaxLength(256);

        // Indexes
        builder.HasIndex(q => q.TopicId).HasDatabaseName("IX_Questions_TopicId");

        builder.HasIndex(q => q.CognitiveLevel).HasDatabaseName("IX_Questions_CognitiveLevel");

        builder.HasIndex(q => q.Type).HasDatabaseName("IX_Questions_Type");

        builder.HasIndex(q => q.IsPublic).HasDatabaseName("IX_Questions_IsPublic");

        builder.HasIndex(q => q.CreatedBy).HasDatabaseName("IX_Questions_CreatedBy");

        // Relationships
        builder
            .HasMany(q => q.Answers)
            .WithOne()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
