using FrogEdu.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Subscription.Infrastructure.Persistence.Configurations;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.ToTable("UserSubscriptions");

        builder.HasKey(us => us.Id);
        builder.Property(us => us.Id).ValueGeneratedNever();

        builder.Property(us => us.UserId).IsRequired();

        builder.Property(us => us.SubscriptionTierId).IsRequired();

        builder.Property(us => us.StartDate).IsRequired();

        builder.Property(us => us.EndDate).IsRequired();

        builder.Property(us => us.Status).IsRequired().HasConversion<string>().HasMaxLength(50);

        // Indexes
        builder.HasIndex(us => us.UserId).HasDatabaseName("IX_UserSubscriptions_UserId");

        builder
            .HasIndex(us => us.SubscriptionTierId)
            .HasDatabaseName("IX_UserSubscriptions_SubscriptionTierId");

        builder.HasIndex(us => us.Status).HasDatabaseName("IX_UserSubscriptions_Status");

        builder.HasIndex(us => us.EndDate).HasDatabaseName("IX_UserSubscriptions_EndDate");

        // Relationships
        builder
            .HasOne<SubscriptionTier>()
            .WithMany()
            .HasForeignKey(us => us.SubscriptionTierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(us => us.Transactions)
            .WithOne()
            .HasForeignKey(t => t.UserSubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
