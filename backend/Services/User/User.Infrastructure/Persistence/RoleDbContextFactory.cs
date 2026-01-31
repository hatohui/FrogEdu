using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FrogEdu.User.Infrastructure.Persistence;

public class RoleDbContextFactory : IDesignTimeDbContextFactory<RoleDbContext>
{
    public RoleDbContext CreateDbContext(string[] args)
    {
        Env.Load();

        var connectionString =
            Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING")
            ?? "postgresql://root:root@frog-user-db:5432/user?sslmode=disable";

        var optionsBuilder = new DbContextOptionsBuilder<RoleDbContext>().UseNpgsql(
            connectionString,
            options =>
            {
                options.MigrationsHistoryTable("__ef_migration_history");
            }
        );

        return new RoleDbContext(optionsBuilder.Options);
    }
}
