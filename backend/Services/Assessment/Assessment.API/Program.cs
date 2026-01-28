using FrogEdu.Assessment.Infrastructure.Persistence;
using FrogEdu.Shared.Kernel;
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

// HTTPS termination is handled by CloudFront and API Gateway
// app.UseHttpsRedirection();
app.UseRouting();

// Use Lambda-specific CORS middleware for API Gateway Lambda proxy integration
app.UseLambdaCors();

app.UseCors("AllowSpecificOrigins");

// Health check endpoint
// Path is /health because API Gateway strips /api/assessments prefix
app.MapGet(
        "/health",
        () =>
            Results.Ok(
                new
                {
                    status = "healthy",
                    service = "assessment-api",
                    timestamp = DateTime.UtcNow,
                }
            )
    )
    .WithName("HealthCheck")
    .WithOpenApi();

// Explicit public health endpoint for API Gateway /api/assessments/health (no auth)
app.MapGet(
        "/api/assessments/health",
        () =>
            Results.Ok(
                new
                {
                    status = "healthy",
                    service = "assessment-api",
                    timestamp = DateTime.UtcNow,
                }
            )
    )
    .WithName("HealthCheckPublic")
    .WithOpenApi();

// Database health endpoint
// Path is /health/db because API Gateway strips /api/assessments prefix
app.MapGet(
        "/health/db",
        async () =>
        {
            try
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("ASSESSMENT_DB_CONNECTION_STRING")
                    ?? "postgresql://root:root@frog-assessment-db:5434/assessment?sslmode=disable";

                var options = new DbContextOptionsBuilder<AssessmentDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                await using var ctx = new AssessmentDbContext(options);
                var canConnect = await ctx.Database.CanConnectAsync();

                return canConnect
                    ? Results.Ok(
                        new
                        {
                            status = "healthy",
                            service = "assessment-db",
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

// Explicit public DB health endpoint for API Gateway /api/assessments/health/db (no auth)
app.MapGet(
        "/api/assessments/health/db",
        async () =>
        {
            try
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("ASSESSMENT_DB_CONNECTION_STRING")
                    ?? "postgresql://root:root@frog-assessment-db:5434/assessment?sslmode=disable";

                var options = new DbContextOptionsBuilder<AssessmentDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                await using var ctx = new AssessmentDbContext(options);
                var canConnect = await ctx.Database.CanConnectAsync();

                return canConnect
                    ? Results.Ok(
                        new
                        {
                            status = "healthy",
                            service = "assessment-db",
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
    .WithName("HealthCheckDbPublic")
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
