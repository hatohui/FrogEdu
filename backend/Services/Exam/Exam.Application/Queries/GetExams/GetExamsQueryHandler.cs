using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExams;

public sealed class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, GetExamsResponse>
{
    private readonly IExamRepository _examRepository;

    public GetExamsQueryHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<GetExamsResponse> Handle(
        GetExamsQuery request,
        CancellationToken cancellationToken
    )
    {
        // TODO: Get user ID from authentication context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        IReadOnlyList<Domain.Entities.Exam> exams;

        if (request.IsDraft.HasValue && request.IsDraft.Value)
        {
            exams = await _examRepository.GetDraftExamsAsync(userId, cancellationToken);
        }
        else if (request.IsDraft.HasValue && !request.IsDraft.Value)
        {
            exams = await _examRepository.GetActiveExamsAsync(cancellationToken);
        }
        else
        {
            exams = await _examRepository.GetByCreatorAsync(userId, cancellationToken);
        }

        var examDtos = exams
            .Select(e => new ExamDto(
                e.Id,
                e.Title,
                e.Duration,
                e.PassScore,
                e.MaxAttempts,
                e.StartTime,
                e.EndTime,
                e.TopicId,
                e.IsDraft,
                e.IsActive,
                e.AccessCode,
                e.ExamQuestions.Count,
                e.CreatedAt,
                e.UpdatedAt
            ))
            .ToList();

        return new GetExamsResponse(examDtos);
    }
}
