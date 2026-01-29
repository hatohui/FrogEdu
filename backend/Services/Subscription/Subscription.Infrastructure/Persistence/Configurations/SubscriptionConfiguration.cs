using FrogEdu.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Subscription.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId).IsRequired();

        builder.HasIndex(s => s.UserId);

        builder.Property(s => s.PlanType).IsRequired().HasConversion<int>();

        builder.Property(s => s.Provider).IsRequired().HasConversion<int>();

        builder.Property(s => s.Status).IsRequired().HasConversion<int>();

        builder.Property(s => s.StartDate).IsRequired();

        builder
            .HasMany<Transaction>()
            .WithOne(t => t.Subscription)
            .HasForeignKey(t => t.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
