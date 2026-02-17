using DotNetEnv;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.User.API.Middleware;
using FrogEdu.User.Application;
using FrogEdu.User.Domain.Enums;
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

    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
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

// CORS - Required for API Gateway
builder.Services.AddDevelopmentCors();

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
app.UseDevelopmentCors();

app.UseAuthentication();
app.UseRoleEnrichment(); // Enrich authenticated requests with role claims from User service
app.UseSubscriptionEnrichment(); // Enrich authenticated requests with subscription claims
app.UseAuthorization();

// ============================================================================
// Endpoint Mapping
// ============================================================================

// Attribute-routed controllers
app.MapControllers();

app.Run();
