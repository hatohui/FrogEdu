using FrogEdu.Exam.Application.Commands.AddQuestionsToExam;
using FrogEdu.Exam.Application.Commands.AttachMatrixToExam;
using FrogEdu.Exam.Application.Commands.CreateExam;
using FrogEdu.Exam.Application.Commands.DeleteExam;
using FrogEdu.Exam.Application.Commands.DetachMatrixFromExam;
using FrogEdu.Exam.Application.Commands.PublishExam;
using FrogEdu.Exam.Application.Commands.RemoveQuestionFromExam;
using FrogEdu.Exam.Application.Commands.UpdateExam;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.ExportExamToExcel;
using FrogEdu.Exam.Application.Queries.ExportExamToPdf;
using FrogEdu.Exam.Application.Queries.GetExamById;
using FrogEdu.Exam.Application.Queries.GetExamPreview;
using FrogEdu.Exam.Application.Queries.GetExamQuestions;
using FrogEdu.Exam.Application.Queries.GetExams;
using FrogEdu.Exam.Application.Queries.GetExamSessionData;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("exams")]
[Authorize]
public class ExamsController(IMediator mediator, IExamRepository examRepository) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly IExamRepository _examRepository = examRepository;

    [HttpGet]
    [ProducesResponseType(typeof(GetExamsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetExamsResponse>> GetExams(
        [FromQuery] bool? isDraft,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetExamsQuery(isDraft, userId);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{examId:guid}")]
    [ProducesResponseType(typeof(ExamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExamDto>> GetExamById(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetExamByIdQuery(examId, userId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateExamResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateExamResponse>> CreateExam(
        [FromBody] CreateExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateExamCommand(
            request.Name,
            request.Description,
            request.SubjectId,
            request.Grade,
            userId
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetExamById), new { examId = response.Id }, response);
    }

    [HttpPut("{examId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExam(
        [FromRoute] Guid examId,
        [FromBody] UpdateExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new UpdateExamCommand(examId, request.Name, request.Description, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{examId:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishExam(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var isAdmin = GetUserRole() == "Admin";
        var command = new PublishExamCommand(examId, userId, isAdmin);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{examId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExam(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new DeleteExamCommand(examId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    // ========== Exam Questions Management ==========

    [HttpGet("{examId:guid}/questions")]
    [ProducesResponseType(typeof(GetExamQuestionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetExamQuestionsResponse>> GetExamQuestions(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetExamQuestionsQuery(examId, userId);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{examId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddQuestionsToExam(
        [FromRoute] Guid examId,
        [FromBody] AddQuestionsToExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new AddQuestionsToExamCommand(examId, request.QuestionIds, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{examId:guid}/questions/{questionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveQuestionFromExam(
        [FromRoute] Guid examId,
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new RemoveQuestionFromExamCommand(examId, questionId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    // ========== Exam Preview & Export ==========

    [HttpGet("{examId:guid}/preview")]
    [ProducesResponseType(typeof(ExamPreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExamPreviewDto>> GetExamPreview(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetExamPreviewQuery(examId, userId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get exam data with question/answer GUIDs for session grading and student display.
    /// No ownership check — used for service-to-service communication and exam sessions.
    /// </summary>
    [HttpGet("{examId:guid}/session-data")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ExamSessionDataDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExamSessionDataDto>> GetExamSessionData(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetExamSessionDataQuery(examId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // ========== Exam Matrix Management ==========

    /// <summary>
    /// Attach a matrix blueprint to an exam
    /// </summary>
    [HttpPost("{examId:guid}/matrix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AttachMatrixToExam(
        [FromRoute] Guid examId,
        [FromBody] AttachMatrixRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new AttachMatrixToExamCommand(examId, request.MatrixId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Detach the matrix blueprint from an exam
    /// </summary>
    [HttpDelete("{examId:guid}/matrix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DetachMatrixFromExam(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new DetachMatrixFromExamCommand(examId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    // ========== Exam Export ==========

    [HttpGet("{examId:guid}/export/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportExamToPdf(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new ExportExamToPdfQuery(examId, userId);
        var pdfBytes = await _mediator.Send(query, cancellationToken);

        if (pdfBytes is null)
            return NotFound();

        return File(pdfBytes, "application/pdf", $"exam-{examId}.pdf");
    }

    /// <summary>
    /// Internal endpoint — no auth required. Used by the Class service to resolve exam names
    /// for assignment display without exposing full exam details.
    /// </summary>
    [HttpGet("internal/batch-names")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ExamNameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExamNamesBatch(
        [FromQuery] List<Guid> ids,
        CancellationToken cancellationToken
    )
    {
        if (ids is null || ids.Count == 0)
            return Ok(Array.Empty<ExamNameDto>());

        var exams = await _examRepository.GetByIdsAsync(ids, cancellationToken);
        var result = exams.Select(e => new ExamNameDto(e.Id, e.Name)).ToList();
        return Ok(result);
    }

    [HttpGet("{examId:guid}/export/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportExamToExcel(
        [FromRoute] Guid examId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new ExportExamToExcelQuery(examId, userId);
        var excelBytes = await _mediator.Send(query, cancellationToken);

        if (excelBytes is null)
            return NotFound();

        return File(
            excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"exam-{examId}.xlsx"
        );
    }
}
