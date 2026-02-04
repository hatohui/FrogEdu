using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class MatrixConfiguration : IEntityTypeConfiguration<Matrix>
{
    public void Configure(EntityTypeBuilder<Matrix> builder)
    {
        builder.ToTable("Matrices");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        // Standalone matrix properties
        builder.Property(m => m.Name).IsRequired().HasMaxLength(256);
        builder.Property(m => m.Description).HasMaxLength(1000);
        builder.Property(m => m.SubjectId).IsRequired();
        builder.Property(m => m.Grade).IsRequired();

        // Auditable properties
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy);
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.UpdatedBy);

        // Indexes
        builder.HasIndex(m => m.SubjectId).HasDatabaseName("IX_Matrices_SubjectId");
        builder.HasIndex(m => m.Grade).HasDatabaseName("IX_Matrices_Grade");
        builder.HasIndex(m => m.CreatedBy).HasDatabaseName("IX_Matrices_CreatedBy");

        // Relationships
        builder
            .HasMany(m => m.MatrixTopics)
            .WithOne()
            .HasForeignKey(mt => mt.MatrixId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
