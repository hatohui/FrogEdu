using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Class.Infrastructure.Persistence;
using FrogEdu.Class.Infrastructure.Repositories;
using FrogEdu.Class.Infrastructure.Services;
using FrogEdu.Shared.Kernel.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Class.Infrastructure;

public static class DependencyInjection
{
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
            ?? "Host=localhost;Port=5432;Database=class;Username=root;Password=root;SSL Mode=Disable";

        Console.WriteLine("Database Connected.");

        services.AddDbContext<ClassDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ClassDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }
            );
        });

        // Register repositories
        services.AddScoped<IClassRoomRepository, ClassRoomRepository>();
        services.AddScoped<IClassEnrollmentRepository, ClassEnrollmentRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();

        // Register database health service
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

        // Register shared role claims client for role enrichment middleware
        services.AddRoleClaimsClient();

        // Register subscription claims HTTP client for backend token enrichment
        services.AddSubscriptionClaimsClient();

        return services;
    }
}
