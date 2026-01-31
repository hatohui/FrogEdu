using FrogEdu.Subscription.Application.Queries.CheckDatabaseHealth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Subscription.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetHealth()
    {
        return Ok(
            new
            {
                status = "healthy",
                service = "subscription-api",
                timestamp = DateTime.UtcNow,
            }
        );
    }

    [HttpGet("db")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDatabaseHealth(CancellationToken cancellationToken)
    {
        var health = await _mediator.Send(new CheckDatabaseHealthQuery(), cancellationToken);

        return health.IsHealthy
            ? Ok(health)
            : StatusCode(StatusCodes.Status503ServiceUnavailable, health);
    }
}
