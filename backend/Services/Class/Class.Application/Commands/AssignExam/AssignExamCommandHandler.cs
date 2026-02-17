using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.AssignExam;

public sealed class AssignExamCommandHandler
    : IRequestHandler<AssignExamCommand, Result<AssignmentResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILogger<AssignExamCommandHandler> _logger;

    public AssignExamCommandHandler(
        IClassRoomRepository classRoomRepository,
        IAssignmentRepository assignmentRepository,
        ILogger<AssignExamCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _assignmentRepository = assignmentRepository;
        _logger = logger;
    }

    public async Task<Result<AssignmentResponse>> Handle(
        AssignExamCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var classroom = await _classRoomRepository.GetByIdAsync(
                request.ClassId,
                cancellationToken
            );

            if (classroom is null)
            {
                return Result<AssignmentResponse>.Failure("Class not found");
            }

            if (!Guid.TryParse(request.UserId, out var teacherId))
            {
                return Result<AssignmentResponse>.Failure("Invalid user ID format");
            }

            // Only the teacher who owns the class (or admin via controller auth) can assign
            if (classroom.TeacherId != teacherId)
            {
                return Result<AssignmentResponse>.Failure(
                    "Only the class teacher can assign exams"
                );
            }

            if (!classroom.IsActive)
            {
                return Result<AssignmentResponse>.Failure(
                    "Cannot assign exams to an inactive class"
                );
            }

            var assignment = Assignment.Create(
                request.ClassId,
                request.ExamId,
                request.StartDate,
                request.DueDate,
                request.IsMandatory,
                request.Weight
            );

            classroom.AddAssignment(assignment);
            _classRoomRepository.Update(classroom);
            await _classRoomRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Exam {ExamId} assigned to class {ClassId} by teacher {TeacherId}",
                request.ExamId,
                request.ClassId,
                teacherId
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
            _logger.LogWarning(ex, "Validation error while assigning exam");
            return Result<AssignmentResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while assigning exam to class");
            return Result<AssignmentResponse>.Failure("An error occurred while assigning the exam");
        }
    }
}
