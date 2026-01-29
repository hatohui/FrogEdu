using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.Class.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for Class Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class ClassDbContextFactory : IDesignTimeDbContextFactory<ClassDbContext>
{
    public ClassDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("CLASS_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-class-db:5434/class?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<ClassDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new ClassDbContext(optionsBuilder.Options);
    }
}
