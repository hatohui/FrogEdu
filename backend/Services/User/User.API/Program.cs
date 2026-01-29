using DotNetEnv;
using FrogEdu.Shared.Kernel;
using FrogEdu.User.API.Middleware;
using FrogEdu.User.Application;
using FrogEdu.User.Infrastructure;

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
        new Microsoft.OpenApi.Models.OpenApiServer { Url = "/api/users", Description = "User API" }
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
app.UsePathPrefixRewrite("/api/users");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Routing & CORS
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDevelopmentCors();
}

app.UseAuthentication();
app.UseAuthorization();

// ============================================================================
// Endpoint Mapping
// ============================================================================

// Attribute-routed controllers
app.MapControllers();

app.Run();
