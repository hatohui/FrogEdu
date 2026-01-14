using FrogEdu.AI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.AI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for TutorSession aggregate
/// </summary>
public class TutorSessionConfiguration : IEntityTypeConfiguration<TutorSession>
{
    public void Configure(EntityTypeBuilder<TutorSession> builder)
    {
        builder.ToTable("TutorSessions");

        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Id).ValueGeneratedNever();

        builder.Property(ts => ts.StudentId).IsRequired();

        builder.Property(ts => ts.GradeLevel).IsRequired();

        builder.Property(ts => ts.TextbookId);

        builder.Property(ts => ts.ChapterId);

        builder.Property(ts => ts.TotalTokensUsed).IsRequired().HasDefaultValue(0);

        builder.Property(ts => ts.LastActivityAt).IsRequired();

        builder.Property(ts => ts.ExpiresAt);

        builder.Property(ts => ts.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(ts => ts.CreatedAt).IsRequired();

        builder.Property(ts => ts.UpdatedAt).IsRequired();

        builder.Property(ts => ts.CreatedBy).HasMaxLength(256);

        builder.Property(ts => ts.UpdatedBy).HasMaxLength(256);

        builder.Property(ts => ts.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasMany(ts => ts.Messages)
            .WithOne(m => m.TutorSession)
            .HasForeignKey(m => m.TutorSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(ts => ts.StudentId).HasDatabaseName("IX_TutorSessions_StudentId");

        builder
            .HasIndex(ts => ts.LastActivityAt)
            .HasDatabaseName("IX_TutorSessions_LastActivityAt");

        builder
            .HasIndex(ts => new { ts.IsActive, ts.ExpiresAt })
            .HasDatabaseName("IX_TutorSessions_IsActive_ExpiresAt");

        builder.HasIndex(ts => ts.IsDeleted).HasDatabaseName("IX_TutorSessions_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(ts => !ts.IsDeleted);
    }
}
