using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class RubricConfiguration : IEntityTypeConfiguration<Rubric>
{
    public void Configure(EntityTypeBuilder<Rubric> builder)
    {
        builder.ToTable("Rubrics");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.QuestionId).IsRequired();
        builder.Property(r => r.Criteria).IsRequired().HasColumnType("jsonb");
        builder.Property(r => r.Description).HasMaxLength(2000);

        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.UpdatedAt).IsRequired();
        builder.Property(r => r.CreatedBy).HasMaxLength(256);
        builder.Property(r => r.UpdatedBy).HasMaxLength(256);
        builder.Property(r => r.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(r => r.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(r => r.QuestionId).IsUnique().HasDatabaseName("IX_Rubrics_QuestionId");
        builder.HasIndex(r => r.IsDeleted).HasDatabaseName("IX_Rubrics_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
