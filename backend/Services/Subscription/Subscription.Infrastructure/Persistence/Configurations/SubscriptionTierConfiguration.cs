using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Subscription.Infrastructure.Persistence.Configurations;

public class SubscriptionTierConfiguration : IEntityTypeConfiguration<SubscriptionTier>
{
    public void Configure(EntityTypeBuilder<SubscriptionTier> builder)
    {
        builder.ToTable("SubscriptionTiers");

        builder.HasKey(st => st.Id);
        builder.Property(st => st.Id).ValueGeneratedNever();

        builder.Property(st => st.Name).IsRequired().HasMaxLength(100);

        builder.Property(st => st.Description).IsRequired().HasMaxLength(1000);

        builder.Property(st => st.ImageUrl).HasMaxLength(1000);

        // Complex type for Money
        builder.OwnsOne(
            st => st.Price,
            price =>
            {
                price
                    .Property(p => p.Amount)
                    .IsRequired()
                    .HasColumnName("Price")
                    .HasPrecision(18, 2);

                price
                    .Property(p => p.Currency)
                    .IsRequired()
                    .HasColumnName("Currency")
                    .HasMaxLength(10)
                    .HasDefaultValue("VND");
            }
        );

        builder.Property(st => st.DurationInDays).IsRequired();

        builder.Property(st => st.TargetRole).IsRequired().HasMaxLength(50);

        builder.Property(st => st.IsActive).IsRequired().HasDefaultValue(true);

        // Indexes
        builder.HasIndex(st => st.Name).IsUnique().HasDatabaseName("IX_SubscriptionTiers_Name");

        builder.HasIndex(st => st.TargetRole).HasDatabaseName("IX_SubscriptionTiers_TargetRole");

        builder.HasIndex(st => st.IsActive).HasDatabaseName("IX_SubscriptionTiers_IsActive");
    }
}
