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

        builder.Property(s => s.Code).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(256);
        builder.Property(s => s.Grade).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(2000);

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
        builder.Property(s => s.CreatedBy).HasMaxLength(256);
        builder.Property(s => s.UpdatedBy).HasMaxLength(256);
        builder.Property(s => s.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(s => s.Code).IsUnique().HasDatabaseName("IX_Subjects_Code");
        builder.HasIndex(s => s.Grade).HasDatabaseName("IX_Subjects_Grade");
        builder.HasIndex(s => s.IsDeleted).HasDatabaseName("IX_Subjects_IsDeleted");

        // Note: Textbook uses Subject as a value object, not an FK relationship
        // If you want to reference Subject entity, Textbook needs to be refactored

        // Global query filter for soft delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}


