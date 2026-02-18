using FrogEdu.Class.Application.Commands.AdminAssignExam;
using FrogEdu.Class.Application.Commands.AssignExam;
using FrogEdu.Class.Application.Commands.CreateClass;
using FrogEdu.Class.Application.Commands.DeleteAssignment;
using FrogEdu.Class.Application.Commands.JoinClass;
using FrogEdu.Class.Application.Commands.RemoveStudent;
using FrogEdu.Class.Application.Commands.UpdateAssignment;
using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Dtos.requests;
using FrogEdu.Class.Application.Queries.GetClassAssignments;
using FrogEdu.Class.Application.Queries.GetClassDetail;
using FrogEdu.Class.Application.Queries.GetMyClasses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Class.API.Controllers;

[ApiController]
[Route("classes")]
public class ClassController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Get classes for the current user (teacher sees their classes, student sees enrolled classes, admin sees all)
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<ClassSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClasses(CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var role = GetUserRole();

        var logger = HttpContext.RequestServices.GetRequiredService<ILogger<ClassController>>();
        logger.LogInformation("GetClasses - UserId: {UserId}, Role: {Role}", userId, role);

        var query = new GetMyClassesQuery(userId, role);
        var result = await _mediator.Send(query, cancellationToken);

        logger.LogInformation(
            "GetClasses result - Count: {Count}, Role: {Role}",
            result.Count,
            role
        );
        return Ok(result);
    }

    /// <summary>
    /// Get class details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ClassDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetClassDetailQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("Class not found");

        return Ok(result);
    }

    /// <summary>
    /// Create a new class (Teacher only)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateClassResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateClass(
        [FromBody] CreateClassRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new CreateClassCommand(
            request.Name,
            request.Description,
            request.Grade,
            request.MaxStudents,
            userId,
            request.BannerUrl
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(GetClassById),
            new { id = result.Value!.ClassId },
            result.Value
        );
    }

    /// <summary>
    /// Join a class via invite code (Student)
    /// </summary>
    [HttpPost("join")]
    [Authorize]
    [ProducesResponseType(typeof(JoinClassResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinClass(
        [FromBody] JoinClassRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new JoinClassCommand(request.InviteCode, userId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Assign an exam to a class (Teacher who owns the class)
    /// </summary>
    [HttpPost("{classId:guid}/assignments")]
    [Authorize]
    [ProducesResponseType(typeof(ExamSessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignExam(
        Guid classId,
        [FromBody] AssignExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new AssignExamCommand(
            classId,
            request.ExamId,
            request.StartDate,
            request.DueDate,
            request.IsMandatory,
            request.Weight,
            userId,
            request.RetryTimes,
            request.IsRetryable,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswers,
            request.AllowPartialScoring
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetClassAssignments), new { classId }, result.Value);
    }

    /// <summary>
    /// Get assignments for a class
    /// </summary>
    [HttpGet("{classId:guid}/assignments")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<AssignmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClassAssignments(
        Guid classId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetClassAssignmentsQuery(classId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Admin: Assign an exam to any class (bypasses teacher ownership check)
    /// </summary>
    [HttpPost("admin/{classId:guid}/assignments")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ExamSessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdminAssignExam(
        Guid classId,
        [FromBody] AssignExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new AdminAssignExamCommand(
            classId,
            request.ExamId,
            request.StartDate,
            request.DueDate,
            request.IsMandatory,
            request.Weight,
            request.RetryTimes,
            request.IsRetryable,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswers,
            request.AllowPartialScoring
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetClassAssignments), new { classId }, result.Value);
    }

    /// <summary>
    /// Update an assignment and its linked exam session (Teacher who owns the class or Admin)
    /// </summary>
    [HttpPut("{classId:guid}/assignments/{assignmentId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(AssignmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAssignment(
        Guid classId,
        Guid assignmentId,
        [FromBody] AssignExamRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var role = GetUserRole();

        var command = new UpdateAssignmentCommand(
            classId,
            assignmentId,
            request.StartDate,
            request.DueDate,
            request.IsMandatory,
            request.Weight,
            userId,
            role,
            request.RetryTimes,
            request.IsRetryable,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswers,
            request.AllowPartialScoring
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Delete an assignment and its linked exam session (Teacher who owns the class or Admin)
    /// </summary>
    [HttpDelete("{classId:guid}/assignments/{assignmentId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssignment(
        Guid classId,
        Guid assignmentId,
        CancellationToken cancellationToken
    )
    {
        var userId = GetAuthenticatedUserId();
        var role = GetUserRole();

        var command = new DeleteAssignmentCommand(classId, assignmentId, userId, role);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Remove a student from a class (Teacher who owns the class or Admin)
    /// </summary>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStudent(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveStudentCommand(classId, studentId);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return BadRequest("Failed to remove student from class");

        return NoContent();
    }
}
