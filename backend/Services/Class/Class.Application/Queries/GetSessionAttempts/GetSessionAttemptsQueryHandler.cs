using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetSessionAttempts;

public sealed class GetSessionAttemptsQueryHandler
    : IRequestHandler<GetSessionAttemptsQuery, IReadOnlyList<StudentExamAttemptResponse>>
{
    private readonly IStudentExamAttemptRepository _attemptRepository;

    public GetSessionAttemptsQueryHandler(IStudentExamAttemptRepository attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<StudentExamAttemptResponse>> Handle(
        GetSessionAttemptsQuery request,
        CancellationToken cancellationToken
    )
    {
        var attempts = await _attemptRepository.GetBySessionIdAsync(
            request.ExamSessionId,
            cancellationToken
        );

        return attempts
            .Select(a => new StudentExamAttemptResponse(
                a.Id,
                a.ExamSessionId,
                a.StudentId,
                a.StartedAt,
                a.SubmittedAt,
                a.Score,
                a.TotalPoints,
                a.GetScorePercentage(),
                a.AttemptNumber,
                a.Status.ToString(),
                null
            ))
            .ToList();
    }
}
