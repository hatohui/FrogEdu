using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.ToTable("Exams");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TeacherId).IsRequired();

        builder.Property(e => e.MatrixConfig).IsRequired();

        builder.Property(e => e.PdfUrl).HasMaxLength(500);

        builder.Property(e => e.Status).IsRequired().HasConversion<int>();

        builder
            .HasMany(e => e.ExamQuestions)
            .WithOne(eq => eq.Exam)
            .HasForeignKey(eq => eq.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
