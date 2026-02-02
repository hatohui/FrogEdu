using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.ClassId).IsRequired();

        builder.Property(a => a.ExamId).IsRequired();

        builder.Property(a => a.StartDate).IsRequired();

        builder.Property(a => a.DueDate).IsRequired();

        builder.Property(a => a.IsMandatory).IsRequired().HasDefaultValue(true);

        builder.Property(a => a.Weight).IsRequired().HasDefaultValue(100);

        // Indexes
        builder.HasIndex(a => a.ClassId).HasDatabaseName("IX_Assignments_ClassId");

        builder.HasIndex(a => a.ExamId).HasDatabaseName("IX_Assignments_ExamId");

        builder.HasIndex(a => a.DueDate).HasDatabaseName("IX_Assignments_DueDate");
    }
}
