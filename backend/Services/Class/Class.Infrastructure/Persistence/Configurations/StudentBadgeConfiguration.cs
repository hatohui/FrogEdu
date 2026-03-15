using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class StudentBadgeConfiguration : IEntityTypeConfiguration<StudentBadge>
{
    public void Configure(EntityTypeBuilder<StudentBadge> builder)
    {
        builder.ToTable("StudentBadges");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.StudentId).IsRequired();
        builder.Property(e => e.BadgeId).IsRequired();
        builder.Property(e => e.ClassId).IsRequired();
        builder.Property(e => e.ExamSessionId);
        builder.Property(e => e.AwardedByTeacherId);
        builder.Property(e => e.CustomPraise).HasMaxLength(500);
        builder.Property(e => e.AwardedAt).IsRequired();

        builder.HasIndex(e => e.StudentId).HasDatabaseName("IX_StudentBadges_StudentId");
        builder.HasIndex(e => e.BadgeId).HasDatabaseName("IX_StudentBadges_BadgeId");
        builder.HasIndex(e => e.ClassId).HasDatabaseName("IX_StudentBadges_ClassId");
        builder
            .HasIndex(e => new { e.StudentId, e.ClassId })
            .HasDatabaseName("IX_StudentBadges_StudentId_ClassId");
    }
}
