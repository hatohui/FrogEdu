namespace FrogEdu.User.API.Middleware;

/// <summary>
/// Extension methods for CORS configuration
/// </summary>
public static class CorsExtensions
{
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

    public static IApplicationBuilder UseDevelopmentCors(this IApplicationBuilder app)
    {
        app.UseCors();
        return app;
    }
}
