using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExams;

public sealed class GetExamsQueryHandler(IExamRepository examRepository)
    : IRequestHandler<GetExamsQuery, GetExamsResponse>
{
    private readonly IExamRepository _examRepository = examRepository;

    public async Task<GetExamsResponse> Handle(
        GetExamsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(request.UserId);

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
                e.Name,
                e.Description,
                e.TopicId,
                e.SubjectId,
                e.Grade,
                e.IsDraft,
                e.IsActive,
                e.ExamQuestions.Count,
                e.CreatedAt,
                e.UpdatedAt
            ))
            .ToList();

        return new GetExamsResponse(examDtos);
    }
}
