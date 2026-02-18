using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.DeleteAssignment;

public sealed class DeleteAssignmentCommandHandler
    : IRequestHandler<DeleteAssignmentCommand, Result<bool>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly ILogger<DeleteAssignmentCommandHandler> _logger;

    public DeleteAssignmentCommandHandler(
        IClassRoomRepository classRoomRepository,
        IAssignmentRepository assignmentRepository,
        IExamSessionRepository examSessionRepository,
        ILogger<DeleteAssignmentCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _assignmentRepository = assignmentRepository;
        _examSessionRepository = examSessionRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        DeleteAssignmentCommand request,
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
                return Result<bool>.Failure("Assignment not found");

            if (assignment.ClassId != request.ClassId)
                return Result<bool>.Failure("Assignment does not belong to this class");

            // Check ownership unless admin
            if (!request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var classroom = await _classRoomRepository.GetByIdAsync(
                    request.ClassId,
                    cancellationToken
                );

                if (classroom is null)
                    return Result<bool>.Failure("Class not found");

                if (!Guid.TryParse(request.UserId, out var teacherId))
                    return Result<bool>.Failure("Invalid user ID format");

                if (classroom.TeacherId != teacherId)
                    return Result<bool>.Failure("Only the class teacher can delete assignments");
            }

            // Delete the linked ExamSession (matched by ClassId + ExamId)
            var sessions = await _examSessionRepository.GetByClassIdAsync(
                request.ClassId,
                cancellationToken
            );

            var linkedSession = sessions.FirstOrDefault(s => s.ExamId == assignment.ExamId);
            if (linkedSession is not null)
            {
                _examSessionRepository.Delete(linkedSession);
                await _examSessionRepository.SaveChangesAsync(cancellationToken);
            }

            _assignmentRepository.Delete(assignment);
            await _assignmentRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Assignment {AssignmentId} deleted from class {ClassId}",
                request.AssignmentId,
                request.ClassId
            );

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting assignment {AssignmentId}", request.AssignmentId);
            return Result<bool>.Failure("An error occurred while deleting the assignment");
        }
    }
}
