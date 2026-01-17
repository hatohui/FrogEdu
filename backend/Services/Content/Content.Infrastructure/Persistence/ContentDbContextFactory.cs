using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.Content.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for Content Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class ContentDbContextFactory : IDesignTimeDbContextFactory<ContentDbContext>
{
    public ContentDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("CONTENT_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-content-db:5433/content?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<ContentDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new ContentDbContext(optionsBuilder.Options);
    }
}
