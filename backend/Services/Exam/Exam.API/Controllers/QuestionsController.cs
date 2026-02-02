using FrogEdu.Exam.Application.Commands.CreateQuestion;
using FrogEdu.Exam.Application.Commands.DeleteQuestion;
using FrogEdu.Exam.Application.Commands.UpdateQuestion;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetQuestionById;
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
    [ProducesResponseType(typeof(GetQuestionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    [HttpGet("{questionId:guid}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var query = new GetQuestionByIdQuery(questionId, userId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateQuestionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        return CreatedAtAction(nameof(GetQuestionById), new { questionId = response.Id }, response);
    }

    [HttpPut("{questionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] Guid questionId,
        [FromBody] UpdateQuestionRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new UpdateQuestionCommand(
            questionId,
            request.Content,
            request.Point,
            request.Type,
            request.CognitiveLevel,
            request.MediaUrl,
            userId
        );
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{questionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestion(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new DeleteQuestionCommand(questionId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
