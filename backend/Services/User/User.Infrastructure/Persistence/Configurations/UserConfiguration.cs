using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserEntity = FrogEdu.User.Domain.Entities.User;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();
        builder
            .Property(u => u.CognitoId)
            .HasConversion(v => v.Value, v => CognitoUserId.Create(v))
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("CognitoSub");

        builder
            .Property(u => u.Email)
            .HasConversion(v => v.Value, v => Email.Create(v))
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.RoleId).IsRequired().HasColumnName("RoleId");
        builder.Property(u => u.AvatarUrl).HasMaxLength(1000);
        builder.Property(u => u.IsEmailVerified).IsRequired().HasDefaultValue(false);

        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();
        builder.Property(u => u.CreatedBy).HasMaxLength(256);
        builder.Property(u => u.UpdatedBy).HasMaxLength(256);
        builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(u => u.CognitoId).IsUnique().HasDatabaseName("IX_Users_CognitoSub");
        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");
        builder.HasIndex(u => u.RoleId).HasDatabaseName("IX_Users_RoleId");
        builder.HasIndex(u => u.IsDeleted).HasDatabaseName("IX_Users_IsDeleted");
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
