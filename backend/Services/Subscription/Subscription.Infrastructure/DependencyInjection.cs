using FrogEdu.Subscription.Application.Interfaces;
using FrogEdu.Subscription.Infrastructure.Persistence;
using FrogEdu.Subscription.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Subscription.Infrastructure;

/// <summary>
/// Extension methods for configuring Infrastructure layer services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add Infrastructure layer services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure DbContext - handle both null and empty strings
        var configConnectionString = configuration.GetConnectionString("SubscriptionDb");
        var envConnectionString = Environment.GetEnvironmentVariable(
            "SUBSCRIPTION_DB_CONNECTION_STRING"
        );

        var connectionString =
            (!string.IsNullOrWhiteSpace(configConnectionString) ? configConnectionString : null)
            ?? (!string.IsNullOrWhiteSpace(envConnectionString) ? envConnectionString : null)
            ?? "postgresql://root:root@localhost:5432/subscription?sslmode=disable";

        Console.WriteLine("Database Connected.");

        services.AddDbContext<SubscriptionDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }
            );
        });

        // Register database health service
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

        return services;
    }
}
