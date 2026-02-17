using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.Subscription.Application.Interfaces;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Infrastructure.Persistence;
using FrogEdu.Subscription.Infrastructure.Repositories;
using FrogEdu.Subscription.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Subscription.Infrastructure;

public static class DependencyInjection
{
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
                    npgsqlOptions.MigrationsAssembly(
                        typeof(SubscriptionDbContext).Assembly.FullName
                    );
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }
            );
        });

        // Register repositories
        services.AddScoped<ISubscriptionTierRepository, SubscriptionTierRepository>();
        services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // Register database health service
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

        // Register shared role claims client for role enrichment middleware
        services.AddRoleClaimsClient();

        return services;
    }
}
