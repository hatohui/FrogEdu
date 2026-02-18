using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetMySessionAttempts;

public sealed class GetMySessionAttemptsQueryHandler
    : IRequestHandler<GetMySessionAttemptsQuery, IReadOnlyList<StudentExamAttemptResponse>>
{
    private readonly IStudentExamAttemptRepository _attemptRepository;

    public GetMySessionAttemptsQueryHandler(IStudentExamAttemptRepository attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<StudentExamAttemptResponse>> Handle(
        GetMySessionAttemptsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!Guid.TryParse(request.StudentId, out var studentId))
            return [];

        var attempts = await _attemptRepository.GetByStudentAndSessionAsync(
            studentId,
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
