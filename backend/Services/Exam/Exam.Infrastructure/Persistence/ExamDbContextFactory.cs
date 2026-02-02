using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FrogEdu.Exam.Infrastructure.Persistence;

public class ExamDbContextFactory : IDesignTimeDbContextFactory<ExamDbContext>
{
    public ExamDbContext CreateDbContext(string[] args)
    {
        // Load .env file from the API project directory
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Exam.API");
        var envFilePath = Path.Combine(apiProjectPath, ".env");

        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        // Get connection string from environment variable
        var connectionString =
            Environment.GetEnvironmentVariable("EXAM_DB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=exam;Username=root;Password=root;SSL Mode=Disable";

        var optionsBuilder = new DbContextOptionsBuilder<ExamDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ExamDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(3);
                npgsqlOptions.CommandTimeout(30);
            }
        );

        return new ExamDbContext(optionsBuilder.Options);
    }
}
