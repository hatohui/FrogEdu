using FrogEdu.User.Application.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Endpoints;

/// <summary>
/// Authentication webhook endpoints for Cognito integration
/// </summary>
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication").WithOpenApi();

        // POST /api/auth/webhook - Cognito Post-Confirmation trigger
        group
            .MapPost("/webhook", CognitoPostConfirmation)
            .WithName("CognitoPostConfirmation")
            .WithDescription("Webhook for Cognito Post-Confirmation trigger to sync user data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous(); // Secured via API Gateway / Lambda trigger
    }

    /// <summary>
    /// Handle Cognito Post-Confirmation trigger
    /// Creates a user record in our database when a new user confirms their email
    /// </summary>
    private static async Task<IResult> CognitoPostConfirmation(
        [FromBody] CognitoTriggerRequest request,
        IMediator mediator,
        ILogger<Program> logger,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Received Cognito Post-Confirmation webhook for user: {Username}",
            request.Request.UserAttributes.Email
        );

        try
        {
            // Extract user attributes from Cognito event
            var userAttributes = request.Request.UserAttributes;

            var command = new CreateUserCommand(
                CognitoId: userAttributes.Sub,
                Email: userAttributes.Email,
                FirstName: userAttributes.GivenName ?? userAttributes.Email.Split('@')[0],
                LastName: userAttributes.FamilyName ?? "",
                Role: userAttributes.CustomRole ?? "Student"
            );

            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                logger.LogWarning(
                    "Failed to create user for Cognito ID {CognitoId}: {Error}",
                    userAttributes.Sub,
                    result.Error
                );

                // Return original event (required by Cognito)
                return Results.Ok(request);
            }

            logger.LogInformation(
                "Successfully created user {UserId} for Cognito ID {CognitoId}",
                result.Value,
                userAttributes.Sub
            );

            // Return the original event (required by Cognito triggers)
            return Results.Ok(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Cognito Post-Confirmation webhook");

            // Still return the event to allow Cognito to proceed
            return Results.Ok(request);
        }
    }
}

/// <summary>
/// Cognito trigger request model
/// </summary>
public record CognitoTriggerRequest
{
    public required string Version { get; init; }
    public required string TriggerSource { get; init; }
    public required string Region { get; init; }
    public required string UserPoolId { get; init; }
    public required string UserName { get; init; }
    public required CognitoCallerContext CallerContext { get; init; }
    public required CognitoRequest Request { get; init; }
    public CognitoResponse? Response { get; init; }
}

public record CognitoCallerContext
{
    public required string AwsSdkVersion { get; init; }
    public required string ClientId { get; init; }
}

public record CognitoRequest
{
    public required CognitoUserAttributes UserAttributes { get; init; }
}

public record CognitoUserAttributes
{
    public required string Sub { get; init; }
    public required string Email { get; init; }
    public bool EmailVerified { get; init; }

    public string? GivenName { get; init; }
    public string? FamilyName { get; init; }
    public string? Name { get; init; }
    public string? Picture { get; init; }

    /// <summary>
    /// Custom attribute for user role (custom:role)
    /// </summary>
    public string? CustomRole { get; init; }
}

public record CognitoResponse
{
    // Empty for Post-Confirmation trigger
}
