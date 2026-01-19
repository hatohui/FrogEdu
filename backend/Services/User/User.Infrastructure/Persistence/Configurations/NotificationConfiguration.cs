using FrogEdu.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.User.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedNever();

        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.Type).IsRequired().HasMaxLength(100);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(500);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(2000);
        builder.Property(n => n.Payload).HasColumnType("jsonb");
        builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);
        builder.Property(n => n.ReadAt);

        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.UpdatedAt).IsRequired();
        builder.Property(n => n.CreatedBy).HasMaxLength(256);
        builder.Property(n => n.UpdatedBy).HasMaxLength(256);
        builder.Property(n => n.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(n => n.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(n => n.UserId).HasDatabaseName("IX_Notifications_UserId");
        builder
            .HasIndex(n => new { n.UserId, n.IsRead })
            .HasDatabaseName("IX_Notifications_UserId_IsRead");
        builder.HasIndex(n => n.CreatedAt).HasDatabaseName("IX_Notifications_CreatedAt");
        builder.HasIndex(n => n.IsDeleted).HasDatabaseName("IX_Notifications_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
