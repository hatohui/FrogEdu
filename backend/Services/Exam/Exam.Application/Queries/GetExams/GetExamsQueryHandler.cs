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

        var allExamsForAdmin =
            request.Role == "Admin"
                ? await _examRepository.GetAllExamsAsync(cancellationToken)
                : null;

        exams = request.Role switch
        {
            "Admin" => request.IsDraft.HasValue && request.IsDraft.Value
                ? allExamsForAdmin!.Where(e => e.IsDraft).ToList()
            : request.IsDraft.HasValue && !request.IsDraft.Value
                ? allExamsForAdmin!.Where(e => !e.IsDraft && e.IsActive).ToList()
            : allExamsForAdmin!,
            "Student" => await _examRepository.GetActiveExamsAsync(cancellationToken),
            _ => request.IsDraft.HasValue && request.IsDraft.Value // Teacher + default
                ? await _examRepository.GetDraftExamsAsync(userId, cancellationToken)
            : request.IsDraft.HasValue && !request.IsDraft.Value
                ? await _examRepository.GetActiveExamsAsync(cancellationToken)
            : await _examRepository.GetByCreatorAsync(userId, cancellationToken),
        };

        var examDtos = exams
            .Select(e => new ExamDto(
                e.Id,
                e.Name,
                e.Description,
                e.SubjectId,
                e.Grade,
                e.IsDraft,
                e.IsActive,
                e.MatrixId,
                e.ExamQuestions.Count,
                e.CreatedAt,
                e.UpdatedAt
            ))
            .ToList();

        return new GetExamsResponse(examDtos);
    }
}
