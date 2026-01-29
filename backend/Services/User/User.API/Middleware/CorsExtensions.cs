namespace FrogEdu.User.API.Middleware;

/// <summary>
/// Extension methods for CORS configuration
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Add CORS policy allowing all origins (development only)
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
    /// Use CORS middleware
    /// </summary>
    public static IApplicationBuilder UseDevelopmentCors(this IApplicationBuilder app)
    {
        app.UseCors();
        return app;
    }
}
