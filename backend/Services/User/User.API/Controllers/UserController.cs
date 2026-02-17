using System.Security.Claims;
using FrogEdu.User.Application.Commands.DeleteUser;
using FrogEdu.User.Application.Commands.RemoveUserAvatar;
using FrogEdu.User.Application.Commands.UpdateAvatar;
using FrogEdu.User.Application.Commands.UpdateProfile;
using FrogEdu.User.Application.Commands.UpdateUser;
using FrogEdu.User.Application.Commands.UpdateUserRole;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Queries.GetAllUsers;
using FrogEdu.User.Application.Queries.GetUserById;
using FrogEdu.User.Application.Queries.GetUserDashboardStats;
using FrogEdu.User.Application.Queries.GetUserProfile;
using FrogEdu.User.Application.Queries.GetUserStatistics;
using FrogEdu.User.Application.Queries.GetUserWithSubscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("")]
[Tags("Users")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get user profile by Cognito ID (for service-to-service calls)
    /// </summary>
    [HttpGet("by-cognito/{cognitoId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByCognitoId(
        string cognitoId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserProfileQuery(cognitoId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
        {
            return NotFound($"User with Cognito ID {cognitoId} not found");
        }

        return Ok(result);
    }

    [HttpGet("users")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null,
        [FromQuery] string? sortBy = "createdAt",
        [FromQuery] string? sortOrder = "desc",
        CancellationToken cancellationToken = default
    )
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

        var query = new GetAllUsersQuery(page, pageSize, search, role, sortBy, sortOrder);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("users/statistics")]
    [Authorize]
    [ProducesResponseType(typeof(UserStatisticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUserStatistics(CancellationToken cancellationToken)
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

        var query = new GetUserStatisticsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get user dashboard statistics (growth chart, role distribution, verification status)
    /// </summary>
    [HttpGet("users/dashboard-stats")]
    [Authorize]
    [ProducesResponseType(typeof(UserDashboardStatsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUserDashboardStats(CancellationToken cancellationToken)
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

        var query = new GetUserDashboardStatsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("users/{id:guid}")]
    [Authorize]
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

    [HttpPut("users/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

        var command = new UpdateUserCommand(
            id,
            request.FirstName,
            request.LastName,
            request.RoleId
        );
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpPut("users/{id:guid}/role")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole(
        Guid id,
        [FromBody] UpdateUserRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

        var command = new UpdateUserRoleCommand(id, request.RoleId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("users/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var userRole = GetUserRole();
        if (userRole != "Admin")
        {
            return Forbid();
        }

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
