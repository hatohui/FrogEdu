using System.Security.Claims;
using FrogEdu.User.Application.Commands.DeleteUser;
using FrogEdu.User.Application.Commands.RemoveUserAvatar;
using FrogEdu.User.Application.Commands.UpdateAvatar;
using FrogEdu.User.Application.Commands.UpdateProfile;
using FrogEdu.User.Application.Commands.UpdateUserRole;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Queries.GetAllUsers;
using FrogEdu.User.Application.Queries.GetUserById;
using FrogEdu.User.Application.Queries.GetUserProfile;
using FrogEdu.User.Application.Queries.GetUserWithSubscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("")]
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

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("users/{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return Ok(result);
    }

    [HttpPut("users/{id:guid}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole(
        Guid id,
        [FromBody] UpdateUserRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateUserRoleCommand(id, request.RoleId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("users/{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

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
    /// Get current user profile with subscription information
    /// </summary>
    [HttpGet("me/full")]
    [Authorize]
    [ProducesResponseType(typeof(UserWithSubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUserWithSubscription(
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var query = new GetUserWithSubscriptionQuery(cognitoId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("User not found");

        return Ok(result);
    }

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

    private string? GetCognitoId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }

    [HttpDelete("me/avatar")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAvatar(CancellationToken cancellationToken)
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var command = new RemoveUserAvatarCommand(cognitoId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
