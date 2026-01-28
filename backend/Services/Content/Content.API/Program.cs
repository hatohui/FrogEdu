using FrogEdu.Content.Infrastructure.Persistence;
using FrogEdu.Shared.Kernel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        );
    });
}

var app = builder.Build();

// Middleware to strip /api/content prefix from request path
app.Use(
    async (context, next) =>
    {
        var path = context.Request.Path.Value ?? "";
        var prefix = "/api/contents";

        if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            context.Request.Path = path.Substring(prefix.Length);
            if (string.IsNullOrEmpty(context.Request.Path.Value))
            {
                context.Request.Path = "/";
            }
        }

        await next();
    }
);

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.MapGet(
    "/health",
    () =>
        Results.Ok(
            new
            {
                status = "healthy",
                service = "content-api",
                timestamp = DateTime.UtcNow,
            }
        )
);

app.MapControllers();
app.Run();
