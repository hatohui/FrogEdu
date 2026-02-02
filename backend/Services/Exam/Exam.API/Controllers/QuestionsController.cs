using FrogEdu.Exam.Application.Commands.CreateQuestion;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetQuestions;
using FrogEdu.Exam.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("questions")]
[Authorize]
public class QuestionsController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<GetQuestionsResponse>> GetQuestions(
        [FromQuery] Guid? topicId,
        [FromQuery] CognitiveLevel? cognitiveLevel,
        [FromQuery] bool? isPublic,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetQuestionsQuery(topicId, cognitiveLevel, isPublic, userId);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

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
