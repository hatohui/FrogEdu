using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed class CreateExamCommandHandler(
    IExamRepository examRepository,
    ISubjectRepository subjectRepository
) : IRequestHandler<CreateExamCommand, CreateExamResponse>
{
    private readonly IExamRepository _examRepository = examRepository;
    private readonly ISubjectRepository _subjectRepository = subjectRepository;

    public async Task<CreateExamResponse> Handle(
        CreateExamCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);

        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        var exam = Domain.Entities.Exam.Create(
            request.Name,
            request.Description,
            request.SubjectId,
            request.Grade,
            request.UserId
        );

        await _examRepository.AddAsync(exam, cancellationToken);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return new CreateExamResponse(
            exam.Id,
            exam.Name,
            exam.Description,
            exam.SubjectId,
            exam.Grade,
            exam.IsDraft,
            exam.IsActive,
            exam.CreatedAt
        );
    }
}
