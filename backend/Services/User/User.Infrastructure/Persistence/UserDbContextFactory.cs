using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.User.Infrastructure.Persistence;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        // Load .env file from the API project directory
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "User.API");
        var envFilePath = Path.Combine(apiProjectPath, ".env");

        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        var connectionString =
            Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=user;Username=root;Password=root;SSL Mode=Disable";

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
