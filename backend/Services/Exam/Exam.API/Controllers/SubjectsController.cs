using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Queries.GetSubjects;
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
    /// Get topics for a specific subject
    /// </summary>
    [HttpGet("{subjectId}/topics")]
    public async Task<ActionResult<GetTopicsResponse>> GetTopics(
        Guid subjectId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetTopicsQuery(subjectId);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}
