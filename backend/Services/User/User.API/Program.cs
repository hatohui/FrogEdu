using FrogEdu.Shared.Kernel;
using FrogEdu.User.API.Endpoints;
using FrogEdu.User.Application;
using FrogEdu.User.Infrastructure;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "FrogEdu User API", Version = "v1" });
});

// Add AWS Lambda support - enables running both locally and in Lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add Application & Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure JWT Authentication with Cognito
var cognitoRegion = builder.Configuration["AWS:Cognito:Region"] ?? "ap-southeast-1";
var cognitoUserPoolId =
    builder.Configuration["AWS:Cognito:UserPoolId"]
    ?? Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID")
    ?? "";

var authority = $"https://cognito-idp.{cognitoRegion}.amazonaws.com/{cognitoUserPoolId}";

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = false, // Cognito doesn't use audience in access tokens
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<
                    ILogger<Program>
                >();
                logger.LogWarning("Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "TeacherOnly",
        policy =>
            policy.RequireAssertion(context =>
            {
                var role =
                    context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                    ?? context.User.FindFirst("custom:role")?.Value;
                return role?.Equals("Teacher", StringComparison.OrdinalIgnoreCase) == true;
            })
    );

    options.AddPolicy(
        "StudentOnly",
        policy =>
            policy.RequireAssertion(context =>
            {
                var role =
                    context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                    ?? context.User.FindFirst("custom:role")?.Value;
                return role?.Equals("Student", StringComparison.OrdinalIgnoreCase) == true;
            })
    );
});

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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// Use Lambda-specific CORS middleware for API Gateway Lambda proxy integration
app.UseLambdaCors();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapClassEndpoints();

// Health check endpoint
// Path is /health because API Gateway strips /api/users prefix
app.MapGet(
        "/health",
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
    .WithTags("Health")
    .WithOpenApi();

// Database health endpoint
// Path is /health/db because API Gateway strips /api/users prefix
app.MapGet(
        "/health/db",
        async (UserDbContext context) =>
        {
            try
            {
                var canConnect = await context.Database.CanConnectAsync();

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
    .WithTags("Health")
    .WithOpenApi();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
