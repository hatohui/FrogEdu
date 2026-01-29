using FrogEdu.Class.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Class.Infrastructure.Persistence.Configurations;

public class ClassMembershipConfiguration : IEntityTypeConfiguration<ClassMembership>
{
    public void Configure(EntityTypeBuilder<ClassMembership> builder)
    {
        builder.ToTable("ClassMemberships");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ClassId).IsRequired();

        builder.Property(m => m.StudentId).IsRequired();

        builder.Property(m => m.JoinedAt).IsRequired();

        builder.HasIndex(m => new { m.ClassId, m.StudentId }).IsUnique();
    }
}
