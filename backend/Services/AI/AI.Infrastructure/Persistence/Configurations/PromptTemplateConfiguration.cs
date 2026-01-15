using FrogEdu.AI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.AI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PromptTemplate entity
/// </summary>
public class PromptTemplateConfiguration : IEntityTypeConfiguration<PromptTemplate>
{
    public void Configure(EntityTypeBuilder<PromptTemplate> builder)
    {
        builder.ToTable("PromptTemplates");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id).ValueGeneratedNever();

        builder.Property(pt => pt.Name).IsRequired().HasMaxLength(256);

        builder.Property(pt => pt.Template).IsRequired().HasMaxLength(8000);

        builder.Property(pt => pt.Description).HasMaxLength(2000);

        builder.Property(pt => pt.Version).IsRequired().HasDefaultValue(1);

        builder.Property(pt => pt.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(pt => pt.CreatedAt).IsRequired();

        builder.Property(pt => pt.UpdatedAt).IsRequired();

        builder.Property(pt => pt.CreatedBy).HasMaxLength(256);

        builder.Property(pt => pt.UpdatedBy).HasMaxLength(256);

        builder.Property(pt => pt.IsDeleted).IsRequired().HasDefaultValue(false);

        // Indexes
        builder.HasIndex(pt => pt.Name).IsUnique().HasDatabaseName("IX_PromptTemplates_Name");

        builder.HasIndex(pt => pt.IsActive).HasDatabaseName("IX_PromptTemplates_IsActive");

        builder.HasIndex(pt => pt.Version).HasDatabaseName("IX_PromptTemplates_Version");

        builder.HasIndex(pt => pt.IsDeleted).HasDatabaseName("IX_PromptTemplates_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(pt => !pt.IsDeleted);
    }
}
