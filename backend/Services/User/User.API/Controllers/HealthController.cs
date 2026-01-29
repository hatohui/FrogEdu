using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

/// <summary>
/// Health check endpoint for service monitoring
/// </summary>
[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Returns service health status
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetHealth()
    {
        return Ok(
            new
            {
                status = "healthy",
                service = "user-api",
                timestamp = DateTime.UtcNow,
            }
        );
    }
}
