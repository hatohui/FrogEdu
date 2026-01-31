using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.User.Infrastructure.Persistence;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        Env.Load();

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
