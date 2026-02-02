using FrogEdu.Exam.Application.Commands.CreateExam;
using FrogEdu.Exam.Application.DTOs;
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
            request.TopicId,
            request.SubjectId,
            request.Grade,
            userId
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetExams), new { id = response.Id }, response);
    }
}
