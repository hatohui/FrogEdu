using FrogEdu.Exam.Application.Commands.CreateMatrix;
using FrogEdu.Exam.Application.Commands.DeleteMatrix;
using FrogEdu.Exam.Application.Commands.UpdateMatrix;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Interfaces;
using FrogEdu.Exam.Application.Queries.GetExamById;
using FrogEdu.Exam.Application.Queries.GetMatrices;
using FrogEdu.Exam.Application.Queries.GetMatrixByExamId;
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
    private readonly IExamExportService _exportService;

    public MatricesController(IMediator mediator, IExamExportService exportService)
    {
        _mediator = mediator;
        _exportService = exportService;
    }

    /// <summary>
    /// Get all matrices for the current user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetMatricesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetMatricesResponse>> GetMatrices(
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetMatricesQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
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
    /// Get matrix by exam ID
    /// </summary>
    [HttpGet("exam/{examId:guid}")]
    [ProducesResponseType(typeof(MatrixDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatrixDto>> GetMatrixByExamId(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetMatrixByExamIdQuery(examId, userId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a standalone matrix blueprint
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
        var command = new CreateMatrixCommand(
            request.Name,
            request.Description,
            request.SubjectId,
            request.Grade,
            request.MatrixTopics,
            userId
        );
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
        var command = new UpdateMatrixCommand(
            matrixId,
            request.Name,
            request.Description,
            request.MatrixTopics,
            userId
        );
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

    /// <summary>
    /// Export matrix to PDF
    /// </summary>
    [HttpGet("{matrixId:guid}/export/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportMatrixToPdf(
        [FromRoute] Guid matrixId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        // Get matrix
        var matrix = await _mediator.Send(
            new GetMatrixByIdQuery(matrixId, userId),
            cancellationToken
        );

        if (matrix is null)
            return NotFound();

        var pdf = _exportService.ExportMatrixToPdf(
            matrix,
            matrix.Name,
            matrix.SubjectName ?? "Unknown Subject",
            matrix.Grade
        );

        return File(pdf, "application/pdf", $"matrix-{matrix.Name}.pdf");
    }

    /// <summary>
    /// Export matrix to Excel
    /// </summary>
    [HttpGet("{matrixId:guid}/export/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportMatrixToExcel(
        [FromRoute] Guid matrixId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        // Get matrix
        var matrix = await _mediator.Send(
            new GetMatrixByIdQuery(matrixId, userId),
            cancellationToken
        );

        if (matrix is null)
            return NotFound();

        var excel = _exportService.ExportMatrixToExcel(
            matrix,
            matrix.Name,
            matrix.SubjectName ?? "Unknown Subject",
            matrix.Grade
        );

        return File(
            excel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"matrix-{matrix.Name}.xlsx"
        );
    }
}
