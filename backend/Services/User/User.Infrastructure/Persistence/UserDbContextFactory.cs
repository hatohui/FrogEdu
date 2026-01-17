using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.User.Infrastructure.Persistence;

/// <summary>
/// DbContextFactory for User Service
/// Used by EF Core CLI for migrations when the context is in a separate project
/// This allows migrations to be created/updated without the application running
/// </summary>
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-user-db:5432/user?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new UserDbContext(optionsBuilder.Options);
    }
}
