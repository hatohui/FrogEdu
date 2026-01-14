using FrogEdu.Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Assessment.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for QuestionOption entity
/// </summary>
public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.ToTable("QuestionOptions");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.Property(o => o.OptionText).IsRequired().HasMaxLength(2000);

        builder.Property(o => o.IsCorrect).IsRequired();

        builder.Property(o => o.OrderIndex).IsRequired();

        builder.Property(o => o.QuestionId).IsRequired();

        builder.Property(o => o.CreatedAt).IsRequired();

        builder.Property(o => o.UpdatedAt).IsRequired();

        builder.Property(o => o.CreatedBy).HasMaxLength(256);

        builder.Property(o => o.UpdatedBy).HasMaxLength(256);

        builder.Property(o => o.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(o => o.QuestionId).HasDatabaseName("IX_QuestionOptions_QuestionId");

        builder
            .HasIndex(o => new { o.QuestionId, o.OrderIndex })
            .IsUnique()
            .HasDatabaseName("IX_QuestionOptions_QuestionId_OrderIndex");

        builder.HasIndex(o => o.IsDeleted).HasDatabaseName("IX_QuestionOptions_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
