using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ClassEnrollmentConfiguration : IEntityTypeConfiguration<ClassEnrollment>
{
    public void Configure(EntityTypeBuilder<ClassEnrollment> builder)
    {
        builder.ToTable("ClassEnrollments");

        builder.HasKey(ce => ce.Id);
        builder.Property(ce => ce.Id).ValueGeneratedNever();

        builder.Property(ce => ce.ClassId).IsRequired();

        builder.Property(ce => ce.StudentId).IsRequired();

        builder.Property(ce => ce.JoinedAt).IsRequired();

        builder.Property(ce => ce.Status).IsRequired().HasConversion<string>().HasMaxLength(50);

        // Indexes
        builder.HasIndex(ce => ce.ClassId).HasDatabaseName("IX_ClassEnrollments_ClassId");

        builder.HasIndex(ce => ce.StudentId).HasDatabaseName("IX_ClassEnrollments_StudentId");

        builder.HasIndex(ce => ce.Status).HasDatabaseName("IX_ClassEnrollments_Status");

        // Composite index for unique constraint
        builder
            .HasIndex(ce => new { ce.ClassId, ce.StudentId })
            .HasDatabaseName("IX_ClassEnrollments_ClassId_StudentId");
    }
}
