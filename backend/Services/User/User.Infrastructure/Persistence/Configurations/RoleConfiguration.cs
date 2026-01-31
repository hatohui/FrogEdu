using System;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();
        builder.Property(r => r.Name).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(1000).IsRequired(false);
        builder.HasIndex(r => r.Name).IsUnique().HasDatabaseName("IX_Roles_Name");
        builder.HasData(
            new
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Name = UserRole.Teacher,
                Description = "Teacher role",
            },
            new
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                Name = UserRole.Student,
                Description = "Student role",
            },
            new
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                Name = UserRole.Admin,
                Description = "Administrator role",
            }
        );
    }
}
