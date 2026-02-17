using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAttemptDetail;

public sealed class GetAttemptDetailQueryHandler
    : IRequestHandler<GetAttemptDetailQuery, StudentExamAttemptResponse?>
{
    private readonly IStudentExamAttemptRepository _attemptRepository;

    public GetAttemptDetailQueryHandler(IStudentExamAttemptRepository attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<StudentExamAttemptResponse?> Handle(
        GetAttemptDetailQuery request,
        CancellationToken cancellationToken
    )
    {
        var attempt = await _attemptRepository.GetByIdWithAnswersAsync(
            request.AttemptId,
            cancellationToken
        );

        if (attempt is null)
            return null;

        var answers = attempt
            .Answers.Select(a => new StudentAnswerResponse(
                a.Id,
                a.QuestionId,
                a.SelectedAnswerIds,
                a.Score,
                a.IsCorrect,
                a.IsPartiallyCorrect
            ))
            .ToList();

        return new StudentExamAttemptResponse(
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
            answers
        );
    }
}
