using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.Assessment.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for Assessment Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class AssessmentDbContextFactory : IDesignTimeDbContextFactory<AssessmentDbContext>
{
    public AssessmentDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ASSESSMENT_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-assessment-db:5434/assessment?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<AssessmentDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new AssessmentDbContext(optionsBuilder.Options);
    }
}
