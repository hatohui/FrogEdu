using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetExamSessions;

public sealed class GetExamSessionsQueryHandler
    : IRequestHandler<GetExamSessionsQuery, IReadOnlyList<ExamSessionResponse>>
{
    private readonly IExamSessionRepository _examSessionRepository;

    public GetExamSessionsQueryHandler(IExamSessionRepository examSessionRepository)
    {
        _examSessionRepository = examSessionRepository;
    }

    public async Task<IReadOnlyList<ExamSessionResponse>> Handle(
        GetExamSessionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var sessions = await _examSessionRepository.GetByClassIdAsync(
            request.ClassId,
            cancellationToken
        );

        return sessions.Select(MapToResponse).ToList();
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
