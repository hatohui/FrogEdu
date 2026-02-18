using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.StartExamAttempt;

public sealed class StartExamAttemptCommandHandler
    : IRequestHandler<StartExamAttemptCommand, Result<StudentExamAttemptResponse>>
{
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IStudentExamAttemptRepository _attemptRepository;
    private readonly IClassEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<StartExamAttemptCommandHandler> _logger;

    public StartExamAttemptCommandHandler(
        IExamSessionRepository examSessionRepository,
        IStudentExamAttemptRepository attemptRepository,
        IClassEnrollmentRepository enrollmentRepository,
        ILogger<StartExamAttemptCommandHandler> logger
    )
    {
        _examSessionRepository = examSessionRepository;
        _attemptRepository = attemptRepository;
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    public async Task<Result<StudentExamAttemptResponse>> Handle(
        StartExamAttemptCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (!Guid.TryParse(request.StudentId, out var studentId))
                return Result<StudentExamAttemptResponse>.Failure("Invalid student ID format");

            var session = await _examSessionRepository.GetByIdWithAttemptsAsync(
                request.ExamSessionId,
                cancellationToken
            );

            if (session is null)
                return Result<StudentExamAttemptResponse>.Failure("Exam session not found");

            // Admins can attempt any session; others must be enrolled
            if (!request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var enrollment = await _enrollmentRepository.GetByClassAndStudentAsync(
                    session.ClassId,
                    studentId,
                    cancellationToken
                );

                if (enrollment is null || enrollment.Status != Domain.Enums.EnrollmentStatus.Active)
                    return Result<StudentExamAttemptResponse>.Failure(
                        "You are not enrolled in this class"
                    );
            }

            // Check if session is currently active
            if (!session.IsCurrentlyActive())
                return Result<StudentExamAttemptResponse>.Failure(
                    "This exam session is not currently active"
                );

            // Check if student can attempt
            if (!session.CanStudentAttempt(studentId))
                return Result<StudentExamAttemptResponse>.Failure(
                    "You have reached the maximum number of attempts for this exam"
                );

            var attemptCount = await _attemptRepository.GetAttemptCountAsync(
                studentId,
                request.ExamSessionId,
                cancellationToken
            );

            var attempt = StudentExamAttempt.Create(
                request.ExamSessionId,
                studentId,
                attemptCount + 1
            );

            session.AddAttempt(attempt);
            await _attemptRepository.AddAsync(attempt, cancellationToken);
            await _attemptRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} started attempt {AttemptNumber} for session {SessionId}",
                studentId,
                attempt.AttemptNumber,
                request.ExamSessionId
            );

            return Result<StudentExamAttemptResponse>.Success(
                new StudentExamAttemptResponse(
                    attempt.Id,
                    attempt.ExamSessionId,
                    attempt.StudentId,
                    attempt.StartedAt,
                    attempt.SubmittedAt,
                    attempt.Score,
                    attempt.TotalPoints,
                    attempt.GetScorePercentage(),
                    attempt.AttemptNumber,
                    attempt.Status.ToString(),
                    null
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting exam attempt");
            return Result<StudentExamAttemptResponse>.Failure(
                "An error occurred while starting the exam attempt"
            );
        }
    }
}
