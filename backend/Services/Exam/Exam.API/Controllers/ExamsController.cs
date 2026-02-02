using FrogEdu.Exam.Application.Commands.CreateExam;
using FrogEdu.Exam.Application.Queries.GetExams;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("exams")]
[Authorize]
public class ExamsController : BaseController
{
    private readonly IMediator _mediator;

    public ExamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all exams for the current user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetExamsResponse>> GetExams(
        [FromQuery] bool? isDraft,
        CancellationToken cancellationToken
    )
    {
        var query = new GetExamsQuery(isDraft);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Create a new exam (as draft)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CreateExamResponse>> CreateExam(
        [FromBody] CreateExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateExamCommand(
            request.Title,
            request.Duration,
            request.PassScore,
            request.MaxAttempts,
            request.StartTime,
            request.EndTime,
            request.TopicId,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswerOptions,
            userId
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetExams), new { id = response.Id }, response);
    }
}

public record CreateExamRequest(
    string Title,
    int Duration,
    int PassScore,
    int MaxAttempts,
    DateTime StartTime,
    DateTime EndTime,
    Guid TopicId,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswerOptions
);
