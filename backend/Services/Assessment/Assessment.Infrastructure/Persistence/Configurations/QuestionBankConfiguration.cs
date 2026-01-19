using FrogEdu.Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Assessment.Infrastructure.Persistence.Configurations;

public class QuestionBankConfiguration : IEntityTypeConfiguration<QuestionBank>
{
    public void Configure(EntityTypeBuilder<QuestionBank> builder)
    {
        builder.ToTable("QuestionBanks");

        builder.HasKey(qb => qb.Id);
        builder.Property(qb => qb.Id).ValueGeneratedNever();

        builder.Property(qb => qb.OwnerId).IsRequired();
        builder.Property(qb => qb.Name).IsRequired().HasMaxLength(500);
        builder.Property(qb => qb.Description).HasMaxLength(2000);
        builder.Property(qb => qb.IsPublic).IsRequired().HasDefaultValue(false);

        builder.Property(qb => qb.CreatedAt).IsRequired();
        builder.Property(qb => qb.UpdatedAt).IsRequired();
        builder.Property(qb => qb.CreatedBy).HasMaxLength(256);
        builder.Property(qb => qb.UpdatedBy).HasMaxLength(256);
        builder.Property(qb => qb.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(qb => qb.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(qb => qb.OwnerId).HasDatabaseName("IX_QuestionBanks_OwnerId");
        builder.HasIndex(qb => qb.IsPublic).HasDatabaseName("IX_QuestionBanks_IsPublic");
        builder.HasIndex(qb => qb.IsDeleted).HasDatabaseName("IX_QuestionBanks_IsDeleted");

        // Note: Removed _questions relationship - Questions managed separately

        // Global query filter for soft delete
        builder.HasQueryFilter(qb => !qb.IsDeleted);
    }
}
