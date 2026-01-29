using FrogEdu.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Subscription.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId).IsRequired();

        builder.Property(t => t.SubscriptionId).IsRequired();

        builder.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(t => t.Currency).IsRequired().HasMaxLength(3);

        builder.Property(t => t.Provider).IsRequired().HasConversion<int>();

        builder.Property(t => t.Status).IsRequired().HasConversion<int>();

        builder.Property(t => t.TransactionId).HasMaxLength(200);

        builder.Property(t => t.PaymentUrl).HasMaxLength(500);

        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.TransactionId);
    }
}
