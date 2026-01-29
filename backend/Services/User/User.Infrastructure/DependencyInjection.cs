using Amazon.S3;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Infrastructure.Persistence;
using FrogEdu.User.Infrastructure.Repositories;
using FrogEdu.User.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.User.Infrastructure;

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
        // Configure DbContext
        var connectionString =
            configuration.GetConnectionString("UserDb")
            ?? Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@localhost:5432/user?sslmode=disable";

        // Log connection string info (mask password)
        var maskedConnectionString = connectionString.Contains("Password=")
            ? System.Text.RegularExpressions.Regex.Replace(
                connectionString,
                @"Password=([^;]+)",
                "Password=***"
            )
            : connectionString;
        Console.WriteLine($"[DB Config] Connection String: {maskedConnectionString}");
        Console.WriteLine($"[DB Config] Connection String Length: {connectionString.Length} chars");

        services.AddDbContext<UserDbContext>(options =>
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

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserDbContext>());

        // Register database health service
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

        // Configure AWS S3
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        // Register storage service
        services.AddScoped<IStorageService, S3StorageService>();

        return services;
    }
}
