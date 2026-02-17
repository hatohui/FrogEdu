using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetExamSessionDetail;

public sealed class GetExamSessionDetailQueryHandler
    : IRequestHandler<GetExamSessionDetailQuery, ExamSessionResponse?>
{
    private readonly IExamSessionRepository _examSessionRepository;

    public GetExamSessionDetailQueryHandler(IExamSessionRepository examSessionRepository)
    {
        _examSessionRepository = examSessionRepository;
    }

    public async Task<ExamSessionResponse?> Handle(
        GetExamSessionDetailQuery request,
        CancellationToken cancellationToken
    )
    {
        var session = await _examSessionRepository.GetByIdWithAttemptsAsync(
            request.SessionId,
            cancellationToken
        );

        if (session is null)
            return null;

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
            session.Attempts.Count,
            session.CreatedAt
        );
    }
}
