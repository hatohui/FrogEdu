using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.UpdateAssignment;

public sealed class UpdateAssignmentCommandHandler
    : IRequestHandler<UpdateAssignmentCommand, Result<AssignmentResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly ILogger<UpdateAssignmentCommandHandler> _logger;

    public UpdateAssignmentCommandHandler(
        IClassRoomRepository classRoomRepository,
        IAssignmentRepository assignmentRepository,
        IExamSessionRepository examSessionRepository,
        ILogger<UpdateAssignmentCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _assignmentRepository = assignmentRepository;
        _examSessionRepository = examSessionRepository;
        _logger = logger;
    }

    public async Task<Result<AssignmentResponse>> Handle(
        UpdateAssignmentCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var assignment = await _assignmentRepository.GetByIdAsync(
                request.AssignmentId,
                cancellationToken
            );

            if (assignment is null)
                return Result<AssignmentResponse>.Failure("Assignment not found");

            if (assignment.ClassId != request.ClassId)
                return Result<AssignmentResponse>.Failure(
                    "Assignment does not belong to this class"
                );

            Guid actorId = Guid.Empty;

            // Check ownership unless admin
            if (!request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var classroom = await _classRoomRepository.GetByIdAsync(
                    request.ClassId,
                    cancellationToken
                );

                if (classroom is null)
                    return Result<AssignmentResponse>.Failure("Class not found");

                if (!Guid.TryParse(request.UserId, out actorId))
                    return Result<AssignmentResponse>.Failure("Invalid user ID format");

                if (classroom.TeacherId != actorId)
                    return Result<AssignmentResponse>.Failure(
                        "Only the class teacher can update assignments"
                    );
            }
            else
            {
                Guid.TryParse(request.UserId, out actorId);
            }

            // Update the Assignment entity
            assignment.Update(
                request.StartDate,
                request.DueDate,
                request.IsMandatory,
                request.Weight
            );
            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveChangesAsync(cancellationToken);

            // Sync the linked ExamSession
            var sessions = await _examSessionRepository.GetByClassIdAsync(
                request.ClassId,
                cancellationToken
            );

            var linkedSession = sessions.FirstOrDefault(s => s.ExamId == assignment.ExamId);
            if (linkedSession is not null)
            {
                linkedSession.Update(
                    request.StartDate,
                    request.DueDate,
                    request.RetryTimes,
                    request.IsRetryable,
                    request.ShouldShuffleQuestions,
                    request.ShouldShuffleAnswers,
                    request.AllowPartialScoring,
                    actorId
                );
                _examSessionRepository.Update(linkedSession);
                await _examSessionRepository.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation(
                "Assignment {AssignmentId} updated in class {ClassId}",
                request.AssignmentId,
                request.ClassId
            );

            return Result<AssignmentResponse>.Success(
                new AssignmentResponse(
                    assignment.Id,
                    assignment.ClassId,
                    assignment.ExamId,
                    assignment.StartDate,
                    assignment.DueDate,
                    assignment.IsMandatory,
                    assignment.Weight,
                    assignment.IsActive(),
                    assignment.IsOverdue()
                )
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Validation error updating assignment {AssignmentId}",
                request.AssignmentId
            );
            return Result<AssignmentResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating assignment {AssignmentId}", request.AssignmentId);
            return Result<AssignmentResponse>.Failure(
                "An error occurred while updating the assignment"
            );
        }
    }
}
