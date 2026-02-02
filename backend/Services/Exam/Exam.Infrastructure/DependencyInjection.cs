using FrogEdu.Exam.Application.Interfaces;
using FrogEdu.Exam.Domain.Repositories;
using FrogEdu.Exam.Infrastructure.Persistence;
using FrogEdu.Exam.Infrastructure.Repositories;
using FrogEdu.Exam.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Exam.Infrastructure;

public static class DependencyInjection
{
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

        return services;
    }
}
