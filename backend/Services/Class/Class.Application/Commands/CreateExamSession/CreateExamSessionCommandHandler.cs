using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.CreateExamSession;

public sealed class CreateExamSessionCommandHandler
    : IRequestHandler<CreateExamSessionCommand, Result<ExamSessionResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly ILogger<CreateExamSessionCommandHandler> _logger;

    public CreateExamSessionCommandHandler(
        IClassRoomRepository classRoomRepository,
        IExamSessionRepository examSessionRepository,
        ILogger<CreateExamSessionCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _examSessionRepository = examSessionRepository;
        _logger = logger;
    }

    public async Task<Result<ExamSessionResponse>> Handle(
        CreateExamSessionCommand request,
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
                    "Only the class teacher can create exam sessions"
                );

            if (!classroom.IsActive)
                return Result<ExamSessionResponse>.Failure(
                    "Cannot create exam sessions for an inactive class"
                );

            var session = ExamSession.Create(
                request.ClassId,
                request.ExamId,
                request.StartTime,
                request.EndTime,
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
                "Exam session created for exam {ExamId} in class {ClassId} by teacher {TeacherId}",
                request.ExamId,
                request.ClassId,
                teacherId
            );

            return Result<ExamSessionResponse>.Success(MapToResponse(session));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating exam session");
            return Result<ExamSessionResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating exam session");
            return Result<ExamSessionResponse>.Failure(
                "An error occurred while creating the exam session"
            );
        }
    }

    private static ExamSessionResponse MapToResponse(ExamSession session)
    {
        return new ExamSessionResponse(
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
        );
    }
}
