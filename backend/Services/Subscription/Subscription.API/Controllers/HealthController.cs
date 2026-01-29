using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Subscription.API.Controllers;

/// <summary>
/// Health check endpoint for service monitoring
/// </summary>
[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Get service health status
    /// </summary>
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
}
