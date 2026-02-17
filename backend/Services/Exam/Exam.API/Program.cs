using DotNetEnv;
using FrogEdu.Exam.API.Middleware;
using FrogEdu.Exam.Application;
using FrogEdu.Exam.Infrastructure;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Authorization;

Env.Load();

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
        new Microsoft.OpenApi.Models.OpenApiServer { Url = "/api/exams", Description = "Exam API" }
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

// CORS - Required for API Gateway
builder.Services.AddDevelopmentCors();

// ============================================================================
// Middleware & Routing Configuration
// ============================================================================

var app = builder.Build();

// Path rewriting
app.UsePathPrefixRewrite("/api/exams");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Routing & CORS
app.UseRouting();
app.UseDevelopmentCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseRoleEnrichment(); // Enrich authenticated requests with role claims from User service
app.UseAuthorization();

// ============================================================================
// Endpoint Mapping
// ============================================================================

// Attribute-routed controllers
app.MapControllers();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
