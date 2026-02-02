using FrogEdu.Exam.Application.Commands.CreateSubject;
using FrogEdu.Exam.Application.Commands.CreateTopic;
using FrogEdu.Exam.Application.Commands.DeleteSubject;
using FrogEdu.Exam.Application.Commands.DeleteTopic;
using FrogEdu.Exam.Application.Commands.UpdateSubject;
using FrogEdu.Exam.Application.Commands.UpdateTopic;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetSubjectById;
using FrogEdu.Exam.Application.Queries.GetSubjects;
using FrogEdu.Exam.Application.Queries.GetTopicById;
using FrogEdu.Exam.Application.Queries.GetTopics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("subjects")]
[Authorize]
public class SubjectsController : BaseController
{
    private readonly IMediator _mediator;

    public SubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all subjects, optionally filtered by grade
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetSubjectsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetSubjectsResponse>> GetSubjects(
        [FromQuery] int? grade,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSubjectsQuery(grade);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Get subject by ID
    /// </summary>
    [HttpGet("{subjectId:guid}")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubjectDto>> GetSubjectById(
        [FromRoute] Guid subjectId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSubjectByIdQuery(subjectId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new subject (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CreateSubjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CreateSubjectResponse>> CreateSubject(
        [FromBody] CreateSubjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateSubjectCommand(
            request.SubjectCode,
            request.Name,
            request.Description,
            request.Grade,
            request.ImageUrl
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetSubjectById), new { subjectId = response.Id }, response);
    }

    /// <summary>
    /// Update a subject (Admin only)
    /// </summary>
    [HttpPut("{subjectId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubject(
        [FromRoute] Guid subjectId,
        [FromBody] UpdateSubjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateSubjectCommand(
            subjectId,
            request.Name,
            request.Description,
            request.ImageUrl
        );
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete a subject (Admin only)
    /// </summary>
    [HttpDelete("{subjectId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubject(
        [FromRoute] Guid subjectId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteSubjectCommand(subjectId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get topics for a specific subject
    /// </summary>
    [HttpGet("{subjectId:guid}/topics")]
    [ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetTopicsResponse>> GetTopics(
        [FromRoute] Guid subjectId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetTopicsQuery(subjectId);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Get topic by ID
    /// </summary>
    [HttpGet("topics/{topicId:guid}")]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TopicDto>> GetTopicById(
        [FromRoute] Guid topicId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetTopicByIdQuery(topicId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new topic
    /// </summary>
    [HttpPost("{subjectId:guid}/topics")]
    [ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateTopicResponse>> CreateTopic(
        [FromRoute] Guid subjectId,
        [FromBody] CreateTopicRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateTopicCommand(
            request.Title,
            request.Description,
            request.IsCurriculum,
            subjectId,
            userId
        );
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTopicById), new { topicId = response.Id }, response);
    }

    /// <summary>
    /// Update a topic (Owner only)
    /// </summary>
    [HttpPut("topics/{topicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTopic(
        [FromRoute] Guid topicId,
        [FromBody] UpdateTopicRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new UpdateTopicCommand(topicId, request.Title, request.Description, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete a topic (Owner only)
    /// </summary>
    [HttpDelete("topics/{topicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTopic(
        [FromRoute] Guid topicId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new DeleteTopicCommand(topicId, userId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
