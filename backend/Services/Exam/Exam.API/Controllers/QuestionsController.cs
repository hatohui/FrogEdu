using FrogEdu.Exam.Application.Commands.CreateQuestion;
using FrogEdu.Exam.Application.Queries.GetQuestions;
using FrogEdu.Exam.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("questions")]
[Authorize]
public class QuestionsController : BaseController
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get questions by filters
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetQuestionsResponse>> GetQuestions(
        [FromQuery] GetQuestionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Create a new question
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CreateQuestionResponse>> CreateQuestion(
        [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateQuestionCommand(
            request.Content,
            request.Point,
            request.Type,
            request.CognitiveLevel,
            request.Source,
            request.TopicId,
            request.MediaUrl,
            request.IsPublic,
            request.Answers,
            userId
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetQuestions), new { id = response.Id }, response);
    }
}

public record CreateQuestionRequest(
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    QuestionSource Source,
    Guid TopicId,
    string? MediaUrl,
    bool IsPublic,
    List<AnswerDto> Answers
);
