using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.UpdateExamSession;

public sealed class UpdateExamSessionCommandHandler
    : IRequestHandler<UpdateExamSessionCommand, Result<ExamSessionResponse>>
{
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<UpdateExamSessionCommandHandler> _logger;

    public UpdateExamSessionCommandHandler(
        IExamSessionRepository examSessionRepository,
        IClassRoomRepository classRoomRepository,
        ILogger<UpdateExamSessionCommandHandler> logger
    )
    {
        _examSessionRepository = examSessionRepository;
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result<ExamSessionResponse>> Handle(
        UpdateExamSessionCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var session = await _examSessionRepository.GetByIdAsync(
                request.SessionId,
                cancellationToken
            );

            if (session is null)
                return Result<ExamSessionResponse>.Failure("Exam session not found");

            if (!Guid.TryParse(request.UserId, out var userId))
                return Result<ExamSessionResponse>.Failure("Invalid user ID format");

            var classroom = await _classRoomRepository.GetByIdAsync(
                session.ClassId,
                cancellationToken
            );

            if (classroom is null)
                return Result<ExamSessionResponse>.Failure("Class not found");

            if (classroom.TeacherId != userId)
                return Result<ExamSessionResponse>.Failure(
                    "Only the class teacher can update exam sessions"
                );

            session.Update(
                request.StartTime,
                request.EndTime,
                request.RetryTimes,
                request.IsRetryable,
                request.ShouldShuffleQuestions,
                request.ShouldShuffleAnswers,
                request.AllowPartialScoring,
                userId
            );

            _examSessionRepository.Update(session);
            await _examSessionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Exam session {SessionId} updated by {UserId}",
                request.SessionId,
                userId
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
            _logger.LogError(ex, "Error updating exam session");
            return Result<ExamSessionResponse>.Failure(
                "An error occurred while updating the exam session"
            );
        }
    }
}
