using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FrogEdu.Class.Infrastructure.Persistence;

public class ClassDbContextFactory : IDesignTimeDbContextFactory<ClassDbContext>
{
    public ClassDbContext CreateDbContext(string[] args)
    {
        // Load .env file from the API project directory
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Class.API");
        var envFilePath = Path.Combine(apiProjectPath, ".env");

        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        // Get connection string from environment variable
        var connectionString =
            Environment.GetEnvironmentVariable("CLASS_DB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=class;Username=root;Password=root;SSL Mode=Disable";

        var optionsBuilder = new DbContextOptionsBuilder<ClassDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ClassDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(3);
                npgsqlOptions.CommandTimeout(30);
            }
        );

        return new ClassDbContext(optionsBuilder.Options);
    }
}
