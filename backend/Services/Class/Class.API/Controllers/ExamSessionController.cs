using FrogEdu.Class.Application.Commands.CreateExamSession;
using FrogEdu.Class.Application.Commands.DeleteExamSession;
using FrogEdu.Class.Application.Commands.StartExamAttempt;
using FrogEdu.Class.Application.Commands.SubmitExamAttempt;
using FrogEdu.Class.Application.Commands.UpdateExamSession;
using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Dtos.requests;
using FrogEdu.Class.Application.Queries.GetAttemptDetail;
using FrogEdu.Class.Application.Queries.GetExamSessionDetail;
using FrogEdu.Class.Application.Queries.GetExamSessions;
using FrogEdu.Class.Application.Queries.GetMySessionAttempts;
using FrogEdu.Class.Application.Queries.GetSessionAttempts;
using FrogEdu.Class.Application.Queries.GetSessionResults;
using FrogEdu.Class.Application.Queries.GetStudentExamSessions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Class.API.Controllers;

[ApiController]
[Route("exam-sessions")]
public class ExamSessionController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    // ─── Teacher Endpoints ───

    /// <summary>
    /// Create an exam session for a class (Teacher)
    /// </summary>
    [HttpPost("classes/{classId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ExamSessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExamSession(
        Guid classId,
        [FromBody] CreateExamSessionRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new CreateExamSessionCommand(
            classId,
            request.ExamId,
            request.StartTime,
            request.EndTime,
            request.RetryTimes,
            request.IsRetryable,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswers,
            request.AllowPartialScoring,
            userId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(
            nameof(GetExamSessionDetail),
            new { sessionId = result.Value!.Id },
            result.Value
        );
    }

    /// <summary>
    /// Update an exam session (Teacher)
    /// </summary>
    [HttpPut("{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ExamSessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExamSession(
        Guid sessionId,
        [FromBody] UpdateExamSessionRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new UpdateExamSessionCommand(
            sessionId,
            request.StartTime,
            request.EndTime,
            request.RetryTimes,
            request.IsRetryable,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswers,
            request.AllowPartialScoring,
            userId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Delete an exam session (Teacher)
    /// </summary>
    [HttpDelete("{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExamSession(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new DeleteExamSessionCommand(sessionId, userId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Get all exam sessions for a class
    /// </summary>
    [HttpGet("classes/{classId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<ExamSessionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExamSessions(
        Guid classId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetExamSessionsQuery(classId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get exam session details
    /// </summary>
    [HttpGet("{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ExamSessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamSessionDetail(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetExamSessionDetailQuery(sessionId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("Exam session not found");

        return Ok(result);
    }

    /// <summary>
    /// Get all attempts for an exam session (Teacher: see who attempted and scores)
    /// </summary>
    [HttpGet("{sessionId:guid}/attempts")]
    [Authorize]
    [ProducesResponseType(
        typeof(IReadOnlyList<StudentExamAttemptResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetSessionAttempts(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSessionAttemptsQuery(sessionId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get session results with student names and aggregated stats (Teacher)
    /// </summary>
    [HttpGet("{sessionId:guid}/results")]
    [Authorize]
    [ProducesResponseType(typeof(ExamSessionResultsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionResults(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSessionResultsQuery(sessionId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("Exam session not found");

        return Ok(result);
    }

    /// <summary>
    /// Get attempt detail including answers (Teacher or the student who owns the attempt)
    /// </summary>
    [HttpGet("attempts/{attemptId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(StudentExamAttemptResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttemptDetail(
        Guid attemptId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetAttemptDetailQuery(attemptId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("Attempt not found");

        return Ok(result);
    }

    /// <summary>
    /// Get all of the current student's own attempts for a session, including scores.
    /// </summary>
    [HttpGet("{sessionId:guid}/my-attempts")]
    [Authorize]
    [ProducesResponseType(
        typeof(IReadOnlyList<StudentExamAttemptResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetMySessionAttempts(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var studentId = GetAuthenticatedUserId();
        var query = new GetMySessionAttemptsQuery(sessionId, studentId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // ─── Student Endpoints ───

    /// <summary>
    /// Get active/upcoming exam sessions for the current student
    /// </summary>
    [HttpGet("student")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<ExamSessionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentExamSessions(
        [FromQuery] bool upcomingOnly = false,
        CancellationToken cancellationToken = default
    )
    {
        var studentId = GetAuthenticatedUserId();
        var role = GetUserRole();

        var query = new GetStudentExamSessionsQuery(studentId, role, upcomingOnly);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Start an exam attempt (Student)
    /// </summary>
    [HttpPost("{sessionId:guid}/attempts")]
    [Authorize]
    [ProducesResponseType(typeof(StudentExamAttemptResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartExamAttempt(
        Guid sessionId,
        CancellationToken cancellationToken
    )
    {
        var studentId = GetAuthenticatedUserId();
        var role = GetUserRole();

        var command = new StartExamAttemptCommand(sessionId, studentId, role);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(
            nameof(GetAttemptDetail),
            new { attemptId = result.Value!.Id },
            result.Value
        );
    }

    /// <summary>
    /// Submit an exam attempt with answers (Student)
    /// </summary>
    [HttpPost("{sessionId:guid}/attempts/{attemptId:guid}/submit")]
    [Authorize]
    [ProducesResponseType(typeof(StudentExamAttemptResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitExamAttempt(
        Guid sessionId,
        Guid attemptId,
        [FromBody] SubmitExamAttemptRequest request,
        CancellationToken cancellationToken
    )
    {
        var studentId = GetAuthenticatedUserId();

        var command = new SubmitExamAttemptCommand(
            sessionId,
            attemptId,
            studentId,
            request.Answers
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
