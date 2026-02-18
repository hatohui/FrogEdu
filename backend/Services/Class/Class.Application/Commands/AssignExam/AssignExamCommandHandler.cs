using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.AssignExam;

public sealed class AssignExamCommandHandler
    : IRequestHandler<AssignExamCommand, Result<ExamSessionResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly ILogger<AssignExamCommandHandler> _logger;

    public AssignExamCommandHandler(
        IClassRoomRepository classRoomRepository,
        IAssignmentRepository assignmentRepository,
        IExamSessionRepository examSessionRepository,
        ILogger<AssignExamCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _assignmentRepository = assignmentRepository;
        _examSessionRepository = examSessionRepository;
        _logger = logger;
    }

    public async Task<Result<ExamSessionResponse>> Handle(
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
                return Result<ExamSessionResponse>.Failure("Class not found");

            if (!Guid.TryParse(request.UserId, out var teacherId))
                return Result<ExamSessionResponse>.Failure("Invalid user ID format");

            if (classroom.TeacherId != teacherId)
                return Result<ExamSessionResponse>.Failure(
                    "Only the class teacher can assign exams"
                );

            if (!classroom.IsActive)
                return Result<ExamSessionResponse>.Failure(
                    "Cannot assign exams to an inactive class"
                );

            var alreadyAssigned = await _assignmentRepository.ExistsAsync(
                request.ClassId,
                request.ExamId,
                cancellationToken
            );
            if (alreadyAssigned)
                return Result<ExamSessionResponse>.Failure(
                    "This exam has already been assigned to the class"
                );

            // Create assignment record for display/grading metadata
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

            // Create the exam session so students can actually take the exam
            var session = ExamSession.Create(
                request.ClassId,
                request.ExamId,
                request.StartDate,
                request.DueDate,
                request.RetryTimes,
                request.IsRetryable,
                request.ShouldShuffleQuestions,
                request.ShouldShuffleAnswers,
                request.AllowPartialScoring,
                teacherId
            );
            await _examSessionRepository.AddAsync(session, cancellationToken);
            await _examSessionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Exam {ExamId} assigned to class {ClassId} by teacher {TeacherId}, session {SessionId} created",
                request.ExamId,
                request.ClassId,
                teacherId,
                session.Id
            );

            return Result<ExamSessionResponse>.Success(
                new ExamSessionResponse(
                    session.Id,
                    session.ClassId,
                    session.ExamId,
                    session.StartTime,
                    session.EndTime,
                    session.RetryTimes,
                    session.IsRetryable,
                    session.IsActive,
                    session.ShouldShuffleQuestions,
                    session.ShouldShuffleAnswers,
                    session.AllowPartialScoring,
                    session.IsCurrentlyActive(),
                    session.IsUpcoming(),
                    session.HasEnded(),
                    0,
                    session.CreatedAt
                )
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while assigning exam");
            return Result<ExamSessionResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while assigning exam to class");
            return Result<ExamSessionResponse>.Failure(
                "An error occurred while assigning the exam"
            );
        }
    }
}
