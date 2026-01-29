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
        // Configure DbContext - handle both null and empty strings
        var configConnectionString = configuration.GetConnectionString("UserDb");
        var envConnectionString = Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING");

        var connectionString =
            (!string.IsNullOrWhiteSpace(configConnectionString) ? configConnectionString : null)
            ?? (!string.IsNullOrWhiteSpace(envConnectionString) ? envConnectionString : null)
            ?? "postgresql://root:root@localhost:5432/user?sslmode=disable";

        Console.WriteLine("Database Connected.");

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

        // Register database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    /// <summary>
    /// Seed the database with initial data
    /// Call this after app.Build() in Program.cs during development
    /// </summary>
    public static async Task SeedDatabaseAsync(
        this IServiceProvider services,
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        FrogEdu.User.Domain.Enums.UserRole role = FrogEdu.User.Domain.Enums.UserRole.Student
    )
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        // Ensure database is created and migrations are applied
        await context.Database.MigrateAsync();

        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedUserFromCognitoAsync(cognitoId, email, firstName, lastName, role);
    }
}
