using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ExamSectionConfiguration : IEntityTypeConfiguration<ExamSection>
{
    public void Configure(EntityTypeBuilder<ExamSection> builder)
    {
        builder.ToTable("ExamSections");

        builder.HasKey(es => es.Id);
        builder.Property(es => es.Id).ValueGeneratedNever();

        builder.Property(es => es.ExamId).IsRequired();
        builder.Property(es => es.OrderIndex).IsRequired();
        builder.Property(es => es.Title).IsRequired().HasMaxLength(500);
        builder.Property(es => es.Instructions).HasMaxLength(2000);

        builder.Property(es => es.CreatedAt).IsRequired();
        builder.Property(es => es.UpdatedAt).IsRequired();
        builder.Property(es => es.CreatedBy).HasMaxLength(256);
        builder.Property(es => es.UpdatedBy).HasMaxLength(256);
        builder.Property(es => es.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(es => es.RowVersion).IsRowVersion();

        // Indexes
        builder
            .HasIndex(es => new { es.ExamId, es.OrderIndex })
            .IsUnique()
            .HasDatabaseName("IX_ExamSections_ExamId_OrderIndex");
        builder.HasIndex(es => es.IsDeleted).HasDatabaseName("IX_ExamSections_IsDeleted");

        // Note: Removed _questions relationship - ExamQuestion is managed separately

        // Global query filter for soft delete
        builder.HasQueryFilter(es => !es.IsDeleted);
    }
}
