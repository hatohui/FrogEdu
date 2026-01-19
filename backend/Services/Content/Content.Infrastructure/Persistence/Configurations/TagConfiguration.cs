using FrogEdu.Content.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Content.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);

        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.Property(t => t.CreatedBy).HasMaxLength(256);
        builder.Property(t => t.UpdatedBy).HasMaxLength(256);
        builder.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(t => t.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(t => t.Name).IsUnique().HasDatabaseName("IX_Tags_Name");
        builder.HasIndex(t => t.IsDeleted).HasDatabaseName("IX_Tags_IsDeleted");

        // Many-to-many with Lessons
        builder.HasMany(t => t.Lessons).WithOne(lt => lt.Tag).HasForeignKey(lt => lt.TagId);

        // Global query filter for soft delete
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}

public class LessonTagConfiguration : IEntityTypeConfiguration<LessonTag>
{
    public void Configure(EntityTypeBuilder<LessonTag> builder)
    {
        builder.ToTable("LessonTags");

        builder.HasKey(lt => new { lt.LessonId, lt.TagId });

        builder
            .HasOne(lt => lt.Lesson)
            .WithMany(l => l.Tags)
            .HasForeignKey(lt => lt.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(lt => lt.Tag)
            .WithMany(t => t.Lessons)
            .HasForeignKey(lt => lt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
