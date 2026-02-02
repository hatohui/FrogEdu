using System.Security.Cryptography.X509Certificates;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Application.Queries.GetRoles;
using MediatR;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
}
