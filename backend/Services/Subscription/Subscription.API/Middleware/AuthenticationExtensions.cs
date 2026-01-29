using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FrogEdu.Subscription.API.Middleware;

/// <summary>
/// Extension methods for authentication setup with Cognito JWT
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Add JWT authentication with Cognito configuration
    /// </summary>
    public static IServiceCollection AddCognitoAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Try multiple sources for configuration values
        var cognitoRegion =
            configuration["AWS:Cognito:Region"]
            ?? Environment.GetEnvironmentVariable("AWS__Cognito__Region")
            ?? Environment.GetEnvironmentVariable("AWS_COGNITO_REGION")
            ?? "ap-southeast-1";

        var cognitoUserPoolId =
            configuration["AWS:Cognito:UserPoolId"]
            ?? Environment.GetEnvironmentVariable("AWS__Cognito__UserPoolId")
            ?? Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID")
            ?? "";

        // Log the configuration for debugging
        Console.WriteLine(
            $"[Cognito Config] Region: {cognitoRegion}, UserPoolId: {cognitoUserPoolId}"
        );

        if (string.IsNullOrEmpty(cognitoUserPoolId))
        {
            Console.WriteLine("[Cognito Config] WARNING: User Pool ID is not configured!");
        }

        var authority = $"https://cognito-idp.{cognitoRegion}.amazonaws.com/{cognitoUserPoolId}";
        Console.WriteLine($"[Cognito Config] Authority: {authority}");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        logger.LogWarning(
                            "Authentication failed: {Error}",
                            context.Exception.Message
                        );
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // Map Cognito custom:role to standard Role claim
                        var identity =
                            context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                        if (identity != null)
                        {
                            var customRole = context.Principal?.FindFirst("custom:role")?.Value;
                            if (!string.IsNullOrEmpty(customRole))
                            {
                                // Add role claim if not already present
                                if (
                                    !identity.HasClaim(c =>
                                        c.Type == System.Security.Claims.ClaimTypes.Role
                                    )
                                )
                                {
                                    identity.AddClaim(
                                        new System.Security.Claims.Claim(
                                            System.Security.Claims.ClaimTypes.Role,
                                            customRole
                                        )
                                    );
                                }
                            }
                        }
                        return Task.CompletedTask;
                    },
                };
            });

        return services;
    }

    /// <summary>
    /// Add authorization policies (kept for backward compatibility, but role-based auth is now preferred)
    /// </summary>
    public static IServiceCollection AddRoleBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
