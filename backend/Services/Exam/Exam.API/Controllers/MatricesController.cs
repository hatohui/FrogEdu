using FrogEdu.Exam.Application.Commands.CreateMatrix;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Exam.API.Controllers;

[ApiController]
[Route("matrices")]
[Authorize]
public class MatricesController : BaseController
{
    private readonly IMediator _mediator;

    public MatricesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create an exam matrix (blueprint)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CreateMatrixResponse>> CreateMatrix(
        [FromBody] CreateMatrixRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateMatrixCommand(request.ExamId, request.MatrixTopics, userId);
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateMatrix), new { id = response.Id }, response);
    }
}

public record CreateMatrixRequest(Guid ExamId, List<MatrixTopicDto> MatrixTopics);
