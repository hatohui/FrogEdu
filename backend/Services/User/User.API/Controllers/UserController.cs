using System.Security.Claims;
using FrogEdu.User.Application.Commands.UpdateAvatar;
using FrogEdu.User.Application.Commands.UpdateProfile;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Queries.GetAvatarUploadUrl;
using FrogEdu.User.Application.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

/// <summary>
/// User profile and authentication management endpoints
/// </summary>
[ApiController]
[Route("api/users")]
[Tags("Users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get current authenticated user's profile
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var query = new GetUserProfileQuery(cognitoId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("User not found");

        return Ok(result);
    }

    /// <summary>
    /// Update current user's profile (first name, last name)
    /// </summary>
    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCurrentUser(
        [FromBody] UpdateProfileDto dto,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var command = new UpdateProfileCommand(cognitoId, dto.FirstName, dto.LastName);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Get a presigned URL for uploading a new avatar
    /// </summary>
    [HttpPost("me/avatar")]
    [Authorize]
    [ProducesResponseType(typeof(AvatarPresignedUrlDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvatarUploadUrl(
        [FromBody] AvatarUploadRequest request,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var query = new GetAvatarUploadUrlQuery(cognitoId, request.ContentType);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("User not found");

        return Ok(result);
    }

    /// <summary>
    /// Confirm avatar upload and update user profile
    /// </summary>
    [HttpPut("me/avatar")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmAvatarUpload(
        [FromBody] ConfirmAvatarRequest request,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var command = new UpdateAvatarCommand(cognitoId, request.AvatarUrl);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Extract Cognito user ID from JWT claims
    /// </summary>
    private string? GetCognitoId()
    {
        // AWS Cognito uses "sub" claim for user ID
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }
}

/// <summary>
/// Request model for avatar upload
/// </summary>
public record AvatarUploadRequest(string ContentType);

/// <summary>
/// Request model for confirming avatar upload
/// </summary>
public record ConfirmAvatarRequest(string AvatarUrl);
