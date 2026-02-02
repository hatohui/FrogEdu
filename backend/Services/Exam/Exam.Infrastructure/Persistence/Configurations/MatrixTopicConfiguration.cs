using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class MatrixTopicConfiguration : IEntityTypeConfiguration<MatrixTopic>
{
    public void Configure(EntityTypeBuilder<MatrixTopic> builder)
    {
        builder.ToTable("MatrixTopics");

        // Composite primary key
        builder.HasKey(mt => new
        {
            mt.MatrixId,
            mt.TopicId,
            mt.CognitiveLevel,
        });

        builder.Property(mt => mt.MatrixId).IsRequired();
        builder.Property(mt => mt.TopicId).IsRequired();

        builder
            .Property(mt => mt.CognitiveLevel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(mt => mt.Quantity).IsRequired();

        // Indexes
        builder.HasIndex(mt => mt.MatrixId).HasDatabaseName("IX_MatrixTopics_MatrixId");

        builder.HasIndex(mt => mt.TopicId).HasDatabaseName("IX_MatrixTopics_TopicId");

        // Relationships
        builder
            .HasOne<Topic>()
            .WithMany()
            .HasForeignKey(mt => mt.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
