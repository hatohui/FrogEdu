using FrogEdu.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserEntity = FrogEdu.User.Domain.Entities.User;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class FullNameConverter : ValueConverter<FullName, string>
{
    public FullNameConverter()
        : base(
            v => $"{v.FirstName}|{v.LastName}",
            v => FullName.Create(v.Substring(0, v.IndexOf('|')), v.Substring(v.IndexOf('|') + 1))
        ) { }
}

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();

        // Value object conversions
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

        // Note: FullName stored as "FirstName|LastName" in database
        builder
            .Property(u => u.FullName)
            .HasConversion(new FullNameConverter())
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(u => u.Role).IsRequired().HasConversion<string>();
        builder.Property(u => u.AvatarUrl).HasMaxLength(1000);
        builder.Property(u => u.LastLoginAt);
        builder.Property(u => u.IsEmailVerified).IsRequired().HasDefaultValue(false);

        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();
        builder.Property(u => u.CreatedBy).HasMaxLength(256);
        builder.Property(u => u.UpdatedBy).HasMaxLength(256);
        builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(u => u.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(u => u.CognitoId).IsUnique().HasDatabaseName("IX_Users_CognitoSub");
        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");
        builder.HasIndex(u => u.Role).HasDatabaseName("IX_Users_Role");
        builder.HasIndex(u => u.IsDeleted).HasDatabaseName("IX_Users_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
