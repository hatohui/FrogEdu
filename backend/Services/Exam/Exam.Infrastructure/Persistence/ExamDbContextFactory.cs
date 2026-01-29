using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.Exam.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for Content Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class ExamDbContextFactory : IDesignTimeDbContextFactory<ExamDbContext>
{
    public ExamDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("EXAM_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-exam-db:5433/exam?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<ExamDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new ExamDbContext(optionsBuilder.Options);
    }
}


