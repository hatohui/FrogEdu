using FrogEdu.AI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.AI.Infrastructure.Persistence.Configurations;

public class SessionEventConfiguration : IEntityTypeConfiguration<SessionEvent>
{
    public void Configure(EntityTypeBuilder<SessionEvent> builder)
    {
        builder.ToTable("SessionEvents");

        builder.HasKey(se => se.Id);
        builder.Property(se => se.Id).ValueGeneratedNever();

        builder.Property(se => se.SessionId).IsRequired();
        builder.Property(se => se.EventType).IsRequired().HasMaxLength(100);
        builder.Property(se => se.Payload).HasColumnType("jsonb");

        builder.Property(se => se.CreatedAt).IsRequired();
        builder.Property(se => se.UpdatedAt).IsRequired();
        builder.Property(se => se.CreatedBy).HasMaxLength(256);
        builder.Property(se => se.UpdatedBy).HasMaxLength(256);
        builder.Property(se => se.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(se => se.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(se => se.SessionId).HasDatabaseName("IX_SessionEvents_SessionId");
        builder.HasIndex(se => se.EventType).HasDatabaseName("IX_SessionEvents_EventType");
        builder.HasIndex(se => se.CreatedAt).HasDatabaseName("IX_SessionEvents_CreatedAt");
        builder.HasIndex(se => se.IsDeleted).HasDatabaseName("IX_SessionEvents_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(se => !se.IsDeleted);
    }
}
