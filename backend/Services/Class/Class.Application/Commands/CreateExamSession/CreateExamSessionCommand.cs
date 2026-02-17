using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.CreateExamSession;

public sealed record CreateExamSessionCommand(
    Guid ClassId,
    Guid ExamId,
    DateTime StartTime,
    DateTime EndTime,
    int RetryTimes,
    bool IsRetryable,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswers,
    bool AllowPartialScoring,
    string UserId
) : IRequest<Result<ExamSessionResponse>>;
