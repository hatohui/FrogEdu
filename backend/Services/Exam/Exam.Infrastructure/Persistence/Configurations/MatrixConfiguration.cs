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

        builder.Property(m => m.ExamId).IsRequired();

        // Auditable properties
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.UpdatedBy).HasMaxLength(256);

        // Indexes
        builder.HasIndex(m => m.ExamId).IsUnique().HasDatabaseName("IX_Matrices_ExamId");

        // Relationships
        builder
            .HasOne<Domain.Entities.Exam>()
            .WithMany()
            .HasForeignKey(m => m.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.MatrixTopics)
            .WithOne()
            .HasForeignKey(mt => mt.MatrixId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
