using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Infrastructure.Persistence;
using FrogEdu.Class.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Class.Infrastructure;

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
        var configConnectionString = configuration.GetConnectionString("ClassDb");
        var envConnectionString = Environment.GetEnvironmentVariable("CLASS_DB_CONNECTION_STRING");

        var connectionString =
            (!string.IsNullOrWhiteSpace(configConnectionString) ? configConnectionString : null)
            ?? (!string.IsNullOrWhiteSpace(envConnectionString) ? envConnectionString : null)
            ?? "postgresql://root:root@localhost:5432/class?sslmode=disable";

        // Log connection string info (mask password)
        Console.WriteLine(
            $"[DB Config] Config value: {(string.IsNullOrWhiteSpace(configConnectionString) ? "null/empty" : "has value")}"
        );
        Console.WriteLine(
            $"[DB Config] Env value: {(string.IsNullOrWhiteSpace(envConnectionString) ? "null/empty" : "has value")}"
        );
        var maskedConnectionString = connectionString.Contains("Password=")
            ? System.Text.RegularExpressions.Regex.Replace(
                connectionString,
                @"Password=([^;]+)",
                "Password=***"
            )
            : connectionString;
        Console.WriteLine($"[DB Config] Connection String: {maskedConnectionString}");
        Console.WriteLine($"[DB Config] Connection String Length: {connectionString.Length} chars");

        services.AddDbContext<ClassDbContext>(options =>
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
