using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Entities.Class>
{
    public void Configure(EntityTypeBuilder<Entities.Class> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.TeacherId).IsRequired();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Subject).IsRequired().HasMaxLength(50);

        builder.Property(c => c.GradeLevel).IsRequired();

        builder.Property(c => c.InviteCode).IsRequired().HasMaxLength(6);

        builder.HasIndex(c => c.InviteCode).IsUnique();

        builder.Property(c => c.IsArchived).IsRequired().HasDefaultValue(false);

        builder
            .HasMany(c => c.Memberships)
            .WithOne(m => m.Class)
            .HasForeignKey(m => m.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
