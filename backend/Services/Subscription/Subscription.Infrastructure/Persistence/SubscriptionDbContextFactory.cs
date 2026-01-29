using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.Subscription.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for AI Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class SubscriptionDbContextFactory : IDesignTimeDbContextFactory<SubscriptionDbContext>
{
    public SubscriptionDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AI_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-ai-db:5435/ai?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<SubscriptionDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new SubscriptionDbContext(optionsBuilder.Options);
    }
}

