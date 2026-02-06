using FrogEdu.Class.Application.Commands.CreateClass;
using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Dtos.requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Class.API.Controllers;

[ApiController]
[Route("classes")]
public class ClassController(IMediator mediator) : BaseController
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
    [Authorize]
    [ProducesResponseType(typeof(CreateClassResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateClass(
        [FromBody] CreateClassRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new CreateClassCommand(
            request.Name,
            request.Description,
            request.Grade,
            request.MaxStudents,
            userId,
            request.BannerUrl
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(GetClassById),
            new { id = result.Value!.ClassId },
            result.Value
        );
    }
}
