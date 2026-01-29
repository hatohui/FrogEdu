using Amazon.Runtime;
using Amazon.S3;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Domain.Services;
using FrogEdu.User.Infrastructure.Persistence;
using FrogEdu.User.Infrastructure.Repositories;
using FrogEdu.User.Infrastructure.Services;
using FrogEdu.User.Infrastructure.Storage;
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

        // Register legacy storage service (for existing avatar upload)
        services.AddScoped<IStorageService, S3StorageService>();

        // Configure R2 (Cloudflare S3-compatible storage)
        ConfigureR2Storage(services, configuration);

        // Register database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    /// <summary>
    /// Configure Cloudflare R2 storage (S3-compatible)
    /// </summary>
    private static void ConfigureR2Storage(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var accountId =
            configuration["R2:AccountId"]
            ?? Environment.GetEnvironmentVariable("R2__AccountId")
            ?? Environment.GetEnvironmentVariable("R2_ACCOUNT_ID");
        var accessKeyId =
            configuration["R2:AccessKeyId"]
            ?? Environment.GetEnvironmentVariable("R2__AccessKeyId")
            ?? Environment.GetEnvironmentVariable("R2_ACCESS_KEY");
        var secretAccessKey =
            configuration["R2:SecretAccessKey"]
            ?? Environment.GetEnvironmentVariable("R2__SecretAccessKey")
            ?? Environment.GetEnvironmentVariable("R2_SECRET_KEY");
        var region =
            configuration["R2:Region"]
            ?? Environment.GetEnvironmentVariable("R2__Region")
            ?? Environment.GetEnvironmentVariable("R2_REGION")
            ?? "auto";

        if (
            string.IsNullOrWhiteSpace(accountId)
            || string.IsNullOrWhiteSpace(accessKeyId)
            || string.IsNullOrWhiteSpace(secretAccessKey)
        )
        {
            Console.WriteLine(
                "Warning: R2 configuration is incomplete. Asset upload will not work."
            );
            return;
        }

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);

            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true,
                AuthenticationRegion = "auto",
            };

            return new AmazonS3Client(credentials, config);
        });

        services.AddScoped<IAssetStorageService, R2AssetStorageService>();

        Console.WriteLine($"R2 Storage configured for account: {accountId}");
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
