var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Health check endpoint
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

await app.RunAsync();
