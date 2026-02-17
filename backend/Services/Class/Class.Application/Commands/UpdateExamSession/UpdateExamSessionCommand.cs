using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.UpdateExamSession;

public sealed record UpdateExamSessionCommand(
    Guid SessionId,
    DateTime StartTime,
    DateTime EndTime,
    int RetryTimes,
    bool IsRetryable,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswers,
    bool AllowPartialScoring,
    string UserId
) : IRequest<Result<ExamSessionResponse>>;
