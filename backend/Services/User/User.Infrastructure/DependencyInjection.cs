using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using FrogEdu.Shared.Kernel.Authorization;
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

public static class DependencyInjection
{
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

        // Register role DbContext and role services
        services.AddDbContext<RoleDbContext>(options =>
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

        services.AddScoped<IRoleService, RoleService>();

        // Configure R2 (Cloudflare S3-compatible storage)
        ConfigureR2Storage(services, configuration);

        // Configure AWS SES for email
        ConfigureSES(services, configuration);

        // Configure AWS Cognito
        ConfigureCognito(services, configuration);

        // Register application-layer interfaces
        services.AddScoped<IPasswordService, CognitoPasswordService>();
        services.AddScoped<ICognitoAttributeService, CognitoAttributeService>();
        services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();

        // Register Subscription Service HTTP client
        // Also register as ISubscriptionClaimsClient for the shared middleware
        services.AddHttpClient<ISubscriptionService, SubscriptionServiceClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
        });
        services.AddScoped<ISubscriptionClaimsClient>(sp =>
            sp.GetRequiredService<ISubscriptionService>()
        );

        // Register shared role claims client for role enrichment middleware
        services.AddRoleClaimsClient();

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
    /// Configure AWS SES for sending emails
    /// </summary>
    private static void ConfigureSES(IServiceCollection services, IConfiguration configuration)
    {
        var accessKeyId =
            configuration["AWS:SES:AccessKeyId"]
            ?? Environment.GetEnvironmentVariable("AWS__SES__AccessKeyId")
            ?? Environment.GetEnvironmentVariable("SES_ACCESS_KEY_ID");
        var secretAccessKey =
            configuration["AWS:SES:SecretAccessKey"]
            ?? Environment.GetEnvironmentVariable("AWS__SES__SecretAccessKey")
            ?? Environment.GetEnvironmentVariable("SES_SECRET_ACCESS_KEY");
        var region =
            configuration["AWS:SES:Region"]
            ?? Environment.GetEnvironmentVariable("AWS__SES__Region")
            ?? Environment.GetEnvironmentVariable("SES_REGION")
            ?? "ap-southeast-1";

        if (string.IsNullOrWhiteSpace(accessKeyId) || string.IsNullOrWhiteSpace(secretAccessKey))
        {
            Console.WriteLine(
                "Warning: SES configuration is incomplete. Email sending will not work."
            );
            services.AddScoped<IEmailService, SesEmailService>();
            return;
        }

        services.AddScoped<IAmazonSimpleEmailService>(sp =>
        {
            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var config = new AmazonSimpleEmailServiceConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region),
            };

            return new AmazonSimpleEmailServiceClient(credentials, config);
        });

        services.AddScoped<IEmailService, SesEmailService>();

        Console.WriteLine($"AWS SES configured for region: {region}");
    }

    /// <summary>
    /// Configure AWS Cognito for authentication
    /// </summary>
    private static void ConfigureCognito(IServiceCollection services, IConfiguration configuration)
    {
        var accessKeyId =
            configuration["AWS:Cognito:AccessKeyId"]
            ?? Environment.GetEnvironmentVariable("AWS__Cognito__AccessKeyId")
            ?? Environment.GetEnvironmentVariable("COGNITO_ACCESS_KEY_ID");
        var secretAccessKey =
            configuration["AWS:Cognito:SecretAccessKey"]
            ?? Environment.GetEnvironmentVariable("AWS__Cognito__SecretAccessKey")
            ?? Environment.GetEnvironmentVariable("COGNITO_SECRET_ACCESS_KEY");
        var region =
            configuration["AWS:Cognito:Region"]
            ?? Environment.GetEnvironmentVariable("AWS__Cognito__Region")
            ?? Environment.GetEnvironmentVariable("COGNITO_REGION")
            ?? "ap-southeast-1";

        if (string.IsNullOrWhiteSpace(accessKeyId) || string.IsNullOrWhiteSpace(secretAccessKey))
        {
            Console.WriteLine(
                "Warning: Cognito configuration is incomplete. Role sync and password reset will not work."
            );
            // Don't register the Cognito client - CognitoAttributeService will handle gracefully
            return;
        }

        services.AddScoped<IAmazonCognitoIdentityProvider>(sp =>
        {
            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var config = new AmazonCognitoIdentityProviderConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region),
            };

            return new AmazonCognitoIdentityProviderClient(credentials, config);
        });

        Console.WriteLine($"AWS Cognito configured for region: {region}");
    }
}
