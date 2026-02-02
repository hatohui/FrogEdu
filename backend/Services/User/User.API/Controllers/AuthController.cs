using FrogEdu.User.Application.Commands.CreateUser;
using FrogEdu.User.Application.Commands.ResetPassword;
using FrogEdu.User.Application.Commands.SendPasswordResetEmail;
using FrogEdu.User.Application.Commands.SendVerificationEmail;
using FrogEdu.User.Application.Commands.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("auth")]
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
        var role = userAttributes.CustomRole ?? "Student";

        var command = new CreateUserCommand(
            userAttributes.Sub,
            userAttributes.Email,
            userAttributes.GivenName ?? "Unknown",
            userAttributes.FamilyName ?? "Unknown",
            role,
            userAttributes.Picture
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

    [HttpPost("send-verification-email/{userId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendVerificationEmail(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Sending verification email for user {UserId}", userId);

        var command = new SendVerificationEmailCommand(userId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "Failed to send verification email for user {UserId}: {Error}",
                userId,
                result.Error
            );
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Verification email sent successfully for user {UserId}", userId);
        return Ok(new { message = "Verification email sent successfully" });
    }

    [HttpPost("verify-email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail(
        [FromBody] VerifyEmailRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Verifying email with token");

        var command = new VerifyEmailCommand(request.Token);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Email verification failed: {Error}", result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Email verified successfully");
        return Ok(new { message = "Email verified successfully" });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Processing forgot password request for email: {Email}",
            request.Email
        );

        var command = new SendPasswordResetEmailCommand(request.Email);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to send password reset email: {Error}", result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Password reset email sent successfully");
        return Ok(new { message = "If the email exists, a password reset link has been sent" });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Processing password reset request");

        var command = new ResetPasswordCommand(request.Token, request.NewPassword);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Password reset failed: {Error}", result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Password reset successfully");
        return Ok(new { message = "Password reset successfully" });
    }
}

public record CognitoTriggerRequest(CognitoTriggerData Request);

public record CognitoTriggerData(CognitoUserAttributes UserAttributes);

public record CognitoUserAttributes(
    string Sub,
    string Email,
    string? GivenName = null,
    string? FamilyName = null,
    string? CustomRole = null,
    string? Picture = null
);

public record VerifyEmailRequest(string Token);

public record ForgotPasswordRequest(string Email);

public record ResetPasswordRequest(string Token, string NewPassword);
