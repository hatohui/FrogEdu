using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.AdminAssignExam;

public sealed record AdminAssignExamCommand(
    Guid ClassId,
    Guid ExamId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory,
    int Weight
) : IRequest<Result<AssignmentResponse>>;

public sealed class AdminAssignExamCommandHandler
    : IRequestHandler<AdminAssignExamCommand, Result<AssignmentResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<AdminAssignExamCommandHandler> _logger;

    public AdminAssignExamCommandHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<AdminAssignExamCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result<AssignmentResponse>> Handle(
        AdminAssignExamCommand request,
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
                "Admin assigned exam {ExamId} to class {ClassId}",
                request.ExamId,
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
            return Result<AssignmentResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while admin assigning exam");
            return Result<AssignmentResponse>.Failure("An error occurred while assigning the exam");
        }
    }
}
