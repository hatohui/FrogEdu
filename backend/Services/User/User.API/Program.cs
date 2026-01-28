using FrogEdu.Shared.Kernel;
using FrogEdu.User.API.Endpoints;
using FrogEdu.User.API.Middleware;
using FrogEdu.User.Application;
using FrogEdu.User.Infrastructure;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
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

app.UsePathPrefixRewrite("/api/users");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapClassEndpoints();

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
);

app.MapControllers();
app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
