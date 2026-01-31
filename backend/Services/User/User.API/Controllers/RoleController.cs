using System.Security.Cryptography.X509Certificates;
using FrogEdu.User.Application.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("roles")]
[Tags("Roles")]
public class RoleController(ILogger<RoleController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<RoleController> _logger = logger;
    private readonly IMediator _mediator = mediator;

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
}
