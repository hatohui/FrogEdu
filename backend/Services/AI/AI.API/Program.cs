using FrogEdu.AI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");

// Health check endpoint
// Path is /health because API Gateway strips /api/ai prefix
app.MapGet(
        "/health",
        () =>
            Results.Ok(
                new
                {
                    status = "healthy",
                    service = "ai-api",
                    timestamp = DateTime.UtcNow,
                }
            )
    )
    .WithName("HealthCheck")
    .WithOpenApi();

// Database health endpoint
// Path is /health/db because API Gateway strips /api/ai prefix
app.MapGet(
        "/health/db",
        async () =>
        {
            try
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("AI_DB_CONNECTION_STRING")
                    ?? "postgresql://root:root@frog-ai-db:5435/ai?sslmode=disable";

                var options = new DbContextOptionsBuilder<AiDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                await using var ctx = new AiDbContext(options);
                var canConnect = await ctx.Database.CanConnectAsync();

                if (canConnect)
                {
                    return Results.Ok(
                        new
                        {
                            status = "healthy",
                            service = "ai-db",
                            timestamp = DateTime.UtcNow,
                        }
                    );
                }
                else
                {
                    return Results.Json(
                        new
                        {
                            status = "unhealthy",
                            service = "ai-db",
                            timestamp = DateTime.UtcNow,
                            error = "Database not reachable",
                        },
                        statusCode: 503
                    );
                }
            }
            catch (Exception ex)
            {
                return Results.Json(
                    new
                    {
                        status = "unhealthy",
                        service = "ai-db",
                        timestamp = DateTime.UtcNow,
                        error = ex.Message,
                    },
                    statusCode: 500
                );
            }
        }
    )
    .WithName("HealthCheckDb")
    .WithOpenApi();

await app.RunAsync();
