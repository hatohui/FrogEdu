using Microsoft.AspNetCore.Http;

namespace FrogEdu.Shared.Kernel;

/// <summary>
/// Middleware to ensure CORS headers are added to all responses when running in AWS Lambda
/// This is necessary because API Gateway Lambda proxy integration requires the Lambda
/// function to return CORS headers in the response.
/// </summary>
public class CorsMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly string[] AllowedOrigins =
    [
        "http://localhost:5173",
        "http://localhost:5174",
        "https://frogedu.org",
        "https://www.frogedu.org",
        "https://api.frogedu.org",
    ];

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var origin = context.Request.Headers["Origin"].ToString();

        // Add CORS headers to all responses
        if (!string.IsNullOrEmpty(origin) && AllowedOrigins.Contains(origin))
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        }
        else if (!string.IsNullOrEmpty(origin))
        {
            // For development, allow localhost with any port
            if (origin.StartsWith("http://localhost:") || origin.StartsWith("https://localhost:"))
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
            }
        }

        context.Response.Headers.Append(
            "Access-Control-Allow-Methods",
            "GET, POST, PUT, DELETE, PATCH, OPTIONS"
        );
        context.Response.Headers.Append(
            "Access-Control-Allow-Headers",
            "Content-Type, Authorization, X-Requested-With, X-Api-Key, X-Amz-Date, X-Amz-Security-Token"
        );
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        context.Response.Headers.Append("Access-Control-Max-Age", "86400"); // 24 hours

        await _next(context);
    }
}

/// <summary>
/// Extension method to register the CORS middleware
/// </summary>
public static class CorsMiddlewareExtensions
{
    public static IApplicationBuilder UseLambdaCors(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}
