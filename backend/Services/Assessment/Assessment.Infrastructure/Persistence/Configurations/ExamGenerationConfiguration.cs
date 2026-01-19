using FrogEdu.Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Assessment.Infrastructure.Persistence.Configurations;

public class ExamGenerationConfiguration : IEntityTypeConfiguration<ExamGeneration>
{
    public void Configure(EntityTypeBuilder<ExamGeneration> builder)
    {
        builder.ToTable("ExamGenerations");

        builder.HasKey(eg => eg.Id);
        builder.Property(eg => eg.Id).ValueGeneratedNever();

        builder.Property(eg => eg.ExamId).IsRequired();
        builder.Property(eg => eg.GeneratedBy).IsRequired();
        builder.Property(eg => eg.Status).IsRequired().HasConversion<int>();
        builder.Property(eg => eg.Prompt).IsRequired().HasMaxLength(4000);
        builder.Property(eg => eg.ResultUri).HasMaxLength(1000);
        builder.Property(eg => eg.Error).HasMaxLength(2000);

        builder.Property(eg => eg.CreatedAt).IsRequired();
        builder.Property(eg => eg.UpdatedAt).IsRequired();
        builder.Property(eg => eg.CreatedBy).HasMaxLength(256);
        builder.Property(eg => eg.UpdatedBy).HasMaxLength(256);
        builder.Property(eg => eg.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(eg => eg.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(eg => eg.ExamId).HasDatabaseName("IX_ExamGenerations_ExamId");
        builder.HasIndex(eg => eg.GeneratedBy).HasDatabaseName("IX_ExamGenerations_GeneratedBy");
        builder.HasIndex(eg => eg.Status).HasDatabaseName("IX_ExamGenerations_Status");
        builder.HasIndex(eg => eg.IsDeleted).HasDatabaseName("IX_ExamGenerations_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(eg => !eg.IsDeleted);
    }
}
