using System.Security.Cryptography.X509Certificates;
using FrogEdu.User.Application.Commands.CreateRole;
using FrogEdu.User.Application.Commands.DeleteRole;
using FrogEdu.User.Application.Commands.UpdateRole;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Application.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("roles")]
[Tags("Roles")]
public class RoleController(
    ILogger<RoleController> logger,
    IMediator mediator,
    IRoleService roleService
) : ControllerBase
{
    private readonly ILogger<RoleController> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [ProducesResponseType(typeof(IReadOnlyList<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var query = new GetRolesQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
        {
            return NotFound("No roles found");
        }
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);

        if (role is null)
        {
            return NotFound($"Role with ID {id} not found");
        }

        return Ok(role);
    }

    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRole(
        [FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateRoleCommand(request.Name, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetRoleById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRole(
        Guid id,
        [FromBody] UpdateRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateRoleCommand(id, request.Name, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}
