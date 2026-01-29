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
        // Configure DbContext
        var connectionString =
            configuration.GetConnectionString("ExamDb")
            ?? Environment.GetEnvironmentVariable("EXAM_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@localhost:5432/exam?sslmode=disable";

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
