using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FrogEdu.Subscription.Infrastructure.Persistence;

public class SubscriptionDbContextFactory : IDesignTimeDbContextFactory<SubscriptionDbContext>
{
    public SubscriptionDbContext CreateDbContext(string[] args)
    {
        // Load .env file from the API project directory
        var apiProjectPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "Subscription.API"
        );
        var envFilePath = Path.Combine(apiProjectPath, ".env");

        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        // Get connection string from environment variable
        var connectionString =
            Environment.GetEnvironmentVariable("SUBSCRIPTION_DB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=subscription;Username=root;Password=root;SSL Mode=Disable";

        var optionsBuilder = new DbContextOptionsBuilder<SubscriptionDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(SubscriptionDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(3);
                npgsqlOptions.CommandTimeout(30);
            }
        );

        return new SubscriptionDbContext(optionsBuilder.Options);
    }
}
