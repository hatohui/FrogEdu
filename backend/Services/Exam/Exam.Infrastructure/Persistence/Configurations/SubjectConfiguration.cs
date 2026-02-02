using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.SubjectCode).IsRequired().HasMaxLength(50);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(256);

        builder.Property(s => s.Description).IsRequired().HasMaxLength(1000);

        builder.Property(s => s.ImageUrl).HasMaxLength(1000);

        builder.Property(s => s.Grade).IsRequired();

        // Indexes
        builder.HasIndex(s => s.SubjectCode).IsUnique().HasDatabaseName("IX_Subjects_SubjectCode");

        builder.HasIndex(s => s.Grade).HasDatabaseName("IX_Subjects_Grade");

        // Relationships
        builder
            .HasMany(s => s.Topics)
            .WithOne()
            .HasForeignKey(t => t.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
