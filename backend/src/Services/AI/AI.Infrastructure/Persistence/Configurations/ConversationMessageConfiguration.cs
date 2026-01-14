using FrogEdu.AI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.AI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ConversationMessage entity
/// </summary>
public class ConversationMessageConfiguration : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.ToTable("ConversationMessages");

        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.Id).ValueGeneratedNever();

        builder.Property(cm => cm.TutorSessionId).IsRequired();

        builder.Property(cm => cm.Role).IsRequired().HasMaxLength(20);

        builder.Property(cm => cm.Content).IsRequired().HasMaxLength(8000);

        builder.Property(cm => cm.TokenCount).IsRequired();

        builder.Property(cm => cm.Timestamp).IsRequired();

        builder.Property(cm => cm.CreatedAt).IsRequired();

        builder.Property(cm => cm.UpdatedAt).IsRequired();

        builder.Property(cm => cm.CreatedBy).HasMaxLength(256);

        builder.Property(cm => cm.UpdatedBy).HasMaxLength(256);

        builder.Property(cm => cm.IsDeleted).IsRequired().HasDefaultValue(false);

        // Configure relationships
        builder
            .HasOne(cm => cm.TutorSession)
            .WithMany(ts => ts.Messages)
            .HasForeignKey(cm => cm.TutorSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder
            .HasIndex(cm => cm.TutorSessionId)
            .HasDatabaseName("IX_ConversationMessages_TutorSessionId");

        builder.HasIndex(cm => cm.Timestamp).HasDatabaseName("IX_ConversationMessages_Timestamp");

        builder.HasIndex(cm => cm.IsDeleted).HasDatabaseName("IX_ConversationMessages_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(cm => !cm.IsDeleted);
    }
}
