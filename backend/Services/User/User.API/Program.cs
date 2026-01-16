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

// Health check endpoint - try multiple path patterns
app.MapGet("/health", () =>
    Results.Ok(new
    {
        status = "healthy",
        service = "user-api",
        path = "/health",
        timestamp = DateTime.UtcNow,
    })
).WithName("HealthCheck").WithOpenApi();

app.MapGet("/users/health", () =>
    Results.Ok(new
    {
        status = "healthy",
        service = "user-api",
        path = "/users/health",
        timestamp = DateTime.UtcNow,
    })
).WithName("HealthCheck2").WithOpenApi();

// Root path test
app.MapGet("/", () =>
    Results.Ok(new
    {
        status = "healthy",
        service = "user-api",
        path = "/",
        message = "Root endpoint",
        timestamp = DateTime.UtcNow,
    })
).WithName("Root").WithOpenApi();

// Diagnostic catch-all route - logs all requests
app.MapFallback((HttpContext context) =>
{
    Console.WriteLine($"[FALLBACK] Method: {context.Request.Method}, Path: {context.Request.Path}, PathBase: {context.Request.PathBase}");
    return Results.Ok(new
    {
        message = "Fallback route",
        path = context.Request.Path.ToString(),
        pathBase = context.Request.PathBase.ToString(),
        method = context.Request.Method
    });
});

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
