using FrogEdu.Exam.Application.Commands.CreateMatrix;
using FrogEdu.Exam.Application.Commands.DeleteMatrix;
using FrogEdu.Exam.Application.Commands.UpdateMatrix;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetMatrixById;
using FrogEdu.Exam.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("matrices")]
[Authorize]
public class MatricesController : BaseController
{
    private readonly IMediator _mediator;

    public MatricesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get matrix by ID
    /// </summary>
    [HttpGet("{matrixId:guid}")]
    [ProducesResponseType(typeof(MatrixDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatrixDto>> GetMatrixById(
        [FromRoute] Guid matrixId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetMatrixByIdQuery(matrixId, userId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create an exam matrix (blueprint)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateMatrixResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateMatrixResponse>> CreateMatrix(
        [FromBody] CreateMatrixRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateMatrixCommand(request.ExamId, request.MatrixTopics, userId);
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetMatrixById), new { matrixId = response.Id }, response);
    }

    /// <summary>
    /// Update an exam matrix (Owner only)
    /// </summary>
    [HttpPut("{matrixId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMatrix(
        [FromRoute] Guid matrixId,
        [FromBody] UpdateMatrixRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new UpdateMatrixCommand(matrixId, request.MatrixTopics, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete an exam matrix (Owner only)
    /// </summary>
    [HttpDelete("{matrixId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMatrix(
        [FromRoute] Guid matrixId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new DeleteMatrixCommand(matrixId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
