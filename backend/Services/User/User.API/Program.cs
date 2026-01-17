using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add AWS Lambda support - enables running both locally and in Lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigins",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:5174",
                    "https://frogedu.org",
                    "https://www.frogedu.org"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");

// Health check endpoint
app.MapGet(
        "/api/users/health",
        () =>
            Results.Ok(
                new
                {
                    status = "healthy",
                    service = "user-api",
                    timestamp = DateTime.UtcNow,
                }
            )
    )
    .WithName("HealthCheck")
    .WithOpenApi();

// Database health endpoint
app.MapGet(
        "/api/users/health/db",
        async () =>
        {
            try
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("USER_DB_CONNECTION_STRING")
                    ?? "postgresql://root:root@frog-user-db:5432/user?sslmode=disable";

                var options = new DbContextOptionsBuilder<UserDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                await using var ctx = new UserDbContext(options);
                var canConnect = await ctx.Database.CanConnectAsync();

                return canConnect
                    ? Results.Ok(
                        new
                        {
                            status = "healthy",
                            service = "user-db",
                            timestamp = DateTime.UtcNow,
                        }
                    )
                    : Results.Problem(title: "Database not reachable", statusCode: 503);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: ex.Message, statusCode: 500);
            }
        }
    )
    .WithName("HealthCheckDb")
    .WithOpenApi();

var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching",
};

app.MapGet(
        "/weatherforecast",
        () =>
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
