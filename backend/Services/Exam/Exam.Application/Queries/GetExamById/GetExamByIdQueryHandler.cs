using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamById;

public sealed class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamDto?>
{
    private readonly IExamRepository _examRepository;

    public GetExamByIdQueryHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<ExamDto?> Handle(
        GetExamByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);

        if (exam is null)
            return null;

        // Verify user has access: Admin sees all; creator sees their own drafts; others only see published
        var userId = Guid.Parse(request.UserId);
        var isAdmin = request.UserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && exam.CreatedBy != userId && !exam.IsActive)
        {
            // Only the creator can view draft exams
            return null;
        }

        return new ExamDto(
            exam.Id,
            exam.Name,
            exam.Description,
            exam.SubjectId,
            exam.Grade,
            exam.IsDraft,
            exam.IsActive,
            exam.MatrixId,
            exam.ExamQuestions.Count,
            exam.CreatedAt,
            exam.UpdatedAt
        );
    }
}
