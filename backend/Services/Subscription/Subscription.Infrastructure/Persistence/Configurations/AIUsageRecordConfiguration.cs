using FrogEdu.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Subscription.Infrastructure.Persistence.Configurations;

public class AIUsageRecordConfiguration : IEntityTypeConfiguration<AIUsageRecord>
{
    public void Configure(EntityTypeBuilder<AIUsageRecord> builder)
    {
        builder.ToTable("AIUsageRecords");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.ActionType).IsRequired().HasMaxLength(100);
        builder.Property(r => r.UsedAt).IsRequired();
        builder.Property(r => r.Metadata).HasMaxLength(1000);

        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => new { r.UserId, r.UsedAt });
    }
}
