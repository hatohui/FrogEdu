using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FrogEdu.User.API.Middleware;

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

        var authority = $"https://cognito-idp.{cognitoRegion}.amazonaws.com/{cognitoUserPoolId}";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = false,
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
                        var identity =
                            context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                        if (identity != null)
                        {
                            var customRole = context.Principal?.FindFirst("custom:role")?.Value;
                            if (!string.IsNullOrEmpty(customRole))
                            {
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
