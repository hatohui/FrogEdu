using FrogEdu.Exam.Application.Interfaces;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using FrogEdu.Exam.Infrastructure.Repositories;
using FrogEdu.Exam.Infrastructure.Services;
using FrogEdu.Shared.Kernel.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace FrogEdu.Exam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;
        var configConnectionString = configuration.GetConnectionString("ExamDb");
        var envConnectionString = Environment.GetEnvironmentVariable("EXAM_DB_CONNECTION_STRING");

        var connectionString =
            (!string.IsNullOrWhiteSpace(configConnectionString) ? configConnectionString : null)
            ?? (!string.IsNullOrWhiteSpace(envConnectionString) ? envConnectionString : null)
            ?? "Host=localhost;Port=5432;Database=exam;Username=root;Password=root;SSL Mode=Disable";

        if (
            connectionString.StartsWith("postgresql://")
            || connectionString.StartsWith("postgres://")
        )
        {
            connectionString = ConvertPostgresUriToConnectionString(connectionString);
        }

        Console.WriteLine("Database Connected.");

        services.AddDbContext<ExamDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ExamDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }
            );
        });

        // Register repositories
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IMatrixRepository, MatrixRepository>();

        // Register database health service
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

        // Register export service
        services.AddScoped<IExamExportService, ExamExportService>();

        // Register subscription claims HTTP client for backend token enrichment
        services.AddSubscriptionClaimsClient();

        return services;
    }

    private static string ConvertPostgresUriToConnectionString(string uri)
    {
        try
        {
            var postgresUri = new Uri(uri);
            var userInfo = postgresUri.UserInfo.Split(':');
            var username = userInfo.Length > 0 ? userInfo[0] : "postgres";
            var password = userInfo.Length > 1 ? userInfo[1] : "";
            var host = postgresUri.Host;
            var port = postgresUri.Port > 0 ? postgresUri.Port : 5432;
            var database = postgresUri.AbsolutePath.TrimStart('/');

            var query = postgresUri.Query;
            var sslMode = query.Contains("sslmode=disable", StringComparison.OrdinalIgnoreCase)
                ? "Disable"
                : "Prefer";

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode={sslMode}";
        }
        catch
        {
            return uri;
        }
    }
}
