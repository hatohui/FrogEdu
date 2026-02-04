using FrogEdu.Exam.Application.Commands.AddQuestionsToExam;
using FrogEdu.Exam.Application.Commands.CreateExam;
using FrogEdu.Exam.Application.Commands.DeleteExam;
using FrogEdu.Exam.Application.Commands.RemoveQuestionFromExam;
using FrogEdu.Exam.Application.Commands.UpdateExam;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetExamById;
using FrogEdu.Exam.Application.Queries.GetExamQuestions;
using FrogEdu.Exam.Application.Queries.GetExams;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("exams")]
[Authorize]
public class ExamsController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

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
}
