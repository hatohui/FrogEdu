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
    int Weight,
    int RetryTimes = 0,
    bool IsRetryable = false,
    bool ShouldShuffleQuestions = false,
    bool ShouldShuffleAnswers = false,
    bool AllowPartialScoring = true
) : IRequest<Result<ExamSessionResponse>>;

public sealed class AdminAssignExamCommandHandler
    : IRequestHandler<AdminAssignExamCommand, Result<ExamSessionResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly ILogger<AdminAssignExamCommandHandler> _logger;

    public AdminAssignExamCommandHandler(
        IClassRoomRepository classRoomRepository,
        IExamSessionRepository examSessionRepository,
        ILogger<AdminAssignExamCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _examSessionRepository = examSessionRepository;
        _logger = logger;
    }

    public async Task<Result<ExamSessionResponse>> Handle(
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
                return Result<ExamSessionResponse>.Failure("Class not found");

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

            // Admin uses class teacher's ID as session creator; fallback to empty guid
            var createdBy = classroom.TeacherId;

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
                createdBy
            );
            await _examSessionRepository.AddAsync(session, cancellationToken);
            await _examSessionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Admin assigned exam {ExamId} to class {ClassId}, session {SessionId} created",
                request.ExamId,
                request.ClassId,
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
            return Result<ExamSessionResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while admin assigning exam");
            return Result<ExamSessionResponse>.Failure(
                "An error occurred while assigning the exam"
            );
        }
    }
}
