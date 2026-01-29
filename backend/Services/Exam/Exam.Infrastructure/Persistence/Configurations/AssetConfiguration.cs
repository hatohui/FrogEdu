using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.LessonId);
        builder.Property(a => a.Kind).IsRequired().HasConversion<int>();
        builder.Property(a => a.Uri).IsRequired().HasMaxLength(1000);
        builder.Property(a => a.MimeType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.ByteSize).IsRequired();
        builder.Property(a => a.Title).HasMaxLength(500);
        builder.Property(a => a.Description).HasMaxLength(2000);

        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.UpdatedAt).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(256);
        builder.Property(a => a.UpdatedBy).HasMaxLength(256);
        builder.Property(a => a.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(a => a.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(a => a.LessonId).HasDatabaseName("IX_Assets_LessonId");
        builder.HasIndex(a => a.Kind).HasDatabaseName("IX_Assets_Kind");
        builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_Assets_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}


