using FrogEdu.User.Application.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

/// <summary>
/// Authentication webhook endpoints for Cognito integration
/// </summary>
[ApiController]
[Route("api/auth")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Cognito Post-Confirmation webhook trigger
    /// Creates a user record in our database when a new user confirms their email
    /// Secured via API Gateway / Lambda trigger (no Bearer token required)
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CognitoPostConfirmation(
        [FromBody] CognitoTriggerRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Received Cognito Post-Confirmation webhook for user: {Username}",
            request.Request.UserAttributes.Email
        );

        if (request?.Request?.UserAttributes == null)
            return BadRequest("Invalid webhook payload");

        var userAttributes = request.Request.UserAttributes;
        // Default to "Student" role if not provided
        var role = userAttributes.CustomRole ?? "Student";

        var command = new CreateUserCommand(
            userAttributes.Sub,
            userAttributes.Email,
            userAttributes.GivenName ?? "Unknown",
            userAttributes.FamilyName ?? "Unknown",
            role
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "Failed to create user for email {Email}: {Error}",
                userAttributes.Email,
                result.Error
            );
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Successfully created user: {UserId}", result.Value);
        return Ok(new { message = "User created successfully", userId = result.Value });
    }
}

/// <summary>
/// Cognito Post-Confirmation trigger request structure
/// </summary>
public record CognitoTriggerRequest(CognitoTriggerData Request);

public record CognitoTriggerData(CognitoUserAttributes UserAttributes);

public record CognitoUserAttributes(
    string Sub,
    string Email,
    string? GivenName = null,
    string? FamilyName = null,
    string? CustomRole = null
);
