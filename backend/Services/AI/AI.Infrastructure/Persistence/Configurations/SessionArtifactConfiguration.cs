using FrogEdu.AI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.AI.Infrastructure.Persistence.Configurations;

public class SessionArtifactConfiguration : IEntityTypeConfiguration<SessionArtifact>
{
    public void Configure(EntityTypeBuilder<SessionArtifact> builder)
    {
        builder.ToTable("SessionArtifacts");

        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id).ValueGeneratedNever();

        builder.Property(sa => sa.SessionId).IsRequired();
        builder.Property(sa => sa.ArtifactType).IsRequired().HasConversion<int>();
        builder.Property(sa => sa.Uri).IsRequired().HasMaxLength(1000);
        builder.Property(sa => sa.Metadata).HasColumnType("jsonb");

        builder.Property(sa => sa.CreatedAt).IsRequired();
        builder.Property(sa => sa.UpdatedAt).IsRequired();
        builder.Property(sa => sa.CreatedBy).HasMaxLength(256);
        builder.Property(sa => sa.UpdatedBy).HasMaxLength(256);
        builder.Property(sa => sa.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(sa => sa.RowVersion).IsRowVersion();

        // Indexes
        builder.HasIndex(sa => sa.SessionId).HasDatabaseName("IX_SessionArtifacts_SessionId");
        builder.HasIndex(sa => sa.ArtifactType).HasDatabaseName("IX_SessionArtifacts_ArtifactType");
        builder.HasIndex(sa => sa.IsDeleted).HasDatabaseName("IX_SessionArtifacts_IsDeleted");

        // Global query filter for soft delete
        builder.HasQueryFilter(sa => !sa.IsDeleted);
    }
}
