namespace FrogEdu.Subscription.API.Middleware;

/// <summary>
/// Extension methods for CORS configuration
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Add CORS policy for development environment
    /// </summary>
    public static IServiceCollection AddDevelopmentCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
        });

        return services;
    }

    /// <summary>
    /// Use CORS middleware for development environment
    /// </summary>
    public static IApplicationBuilder UseDevelopmentCors(this IApplicationBuilder app)
    {
        app.UseCors();
        return app;
    }
}
