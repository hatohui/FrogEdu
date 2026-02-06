using FrogEdu.Class.Application.Dtos.requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Class.API.Controllers;

[ApiController]
[Route("classes")]
public class ClassController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    public IActionResult GetClasses()
    {
        return Ok(new { message = "GetClasses endpoint is not yet implemented." });
    }

    public IActionResult GetClassById(int id)
    {
        return Ok(new { message = $"GetClassById endpoint for ID {id} is not yet implemented." });
    }

    [HttpPost]
    public IActionResult CreateClass(
        [FromBody] CreateClassRequest request,
        CancellationToken cancellationToken
    )
    {
        return Ok(new { message = "CreateClass endpoint is not yet implemented." });
    }
}
