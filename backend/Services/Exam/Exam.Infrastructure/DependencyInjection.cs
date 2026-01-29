using FrogEdu.Exam.Application.Interfaces;
using FrogEdu.Exam.Infrastructure.Persistence;
using FrogEdu.Exam.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Exam.Infrastructure;

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
        var configConnectionString = configuration.GetConnectionString("ExamDb");
        var envConnectionString = Environment.GetEnvironmentVariable("EXAM_DB_CONNECTION_STRING");

        var connectionString =
            (!string.IsNullOrWhiteSpace(configConnectionString) ? configConnectionString : null)
            ?? (!string.IsNullOrWhiteSpace(envConnectionString) ? envConnectionString : null)
            ?? "postgresql://root:root@localhost:5432/exam?sslmode=disable";

        Console.WriteLine("Database Connected.");

        services.AddDbContext<ExamDbContext>(options =>
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
