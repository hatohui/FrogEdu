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
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.TransactionCode).IsRequired().HasMaxLength(100);

        // Complex type for Money
        builder.OwnsOne(
            t => t.Amount,
            amount =>
            {
                amount
                    .Property(a => a.Amount)
                    .IsRequired()
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2);

                amount
                    .Property(a => a.Currency)
                    .IsRequired()
                    .HasColumnName("Currency")
                    .HasMaxLength(10)
                    .HasDefaultValue("VND");
            }
        );

        builder
            .Property(t => t.PaymentProvider)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder
            .Property(t => t.PaymentStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(t => t.ProviderTransactionId).HasMaxLength(256);

        builder.Property(t => t.CreatedAt).IsRequired();

        builder.Property(t => t.UserSubscriptionId).IsRequired();

        // Indexes
        builder
            .HasIndex(t => t.TransactionCode)
            .IsUnique()
            .HasDatabaseName("IX_Transactions_TransactionCode");

        builder
            .HasIndex(t => t.UserSubscriptionId)
            .HasDatabaseName("IX_Transactions_UserSubscriptionId");

        builder.HasIndex(t => t.PaymentStatus).HasDatabaseName("IX_Transactions_PaymentStatus");

        builder.HasIndex(t => t.PaymentProvider).HasDatabaseName("IX_Transactions_PaymentProvider");

        builder.HasIndex(t => t.CreatedAt).HasDatabaseName("IX_Transactions_CreatedAt");
    }
}
