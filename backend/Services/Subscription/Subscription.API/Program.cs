using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.API.Middleware;
using FrogEdu.Subscription.Application;
using FrogEdu.Subscription.Infrastructure;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// Service Configuration
// ============================================================================

// API & OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddServer(
        new Microsoft.OpenApi.Models.OpenApiServer
        {
            Url = "/api/subscriptions",
            Description = "Subscription API",
        }
    );
});

// AWS Lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Authentication & Authorization
builder.Services.AddCognitoAuthentication(builder.Configuration);
builder.Services.AddRoleBasedAuthorization();

// CORS (Development)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevelopmentCors();
}

// ============================================================================
// Middleware & Routing Configuration
// ============================================================================

var app = builder.Build();

// Path rewriting
app.UsePathPrefixRewrite("/api/subscriptions");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Routing & CORS
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDevelopmentCors();
}

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ============================================================================
// Endpoint Mapping
// ============================================================================

// Attribute-routed controllers
app.MapControllers();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
