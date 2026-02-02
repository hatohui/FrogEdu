using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateSubject;

public sealed class CreateSubjectCommandHandler
    : IRequestHandler<CreateSubjectCommand, CreateSubjectResponse>
{
    private readonly ISubjectRepository _subjectRepository;

    public CreateSubjectCommandHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<CreateSubjectResponse> Handle(
        CreateSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if subject code already exists
        var existingSubject = await _subjectRepository.GetByCodeAsync(
            request.SubjectCode,
            cancellationToken
        );
        if (existingSubject is not null)
            throw new InvalidOperationException(
                $"Subject with code '{request.SubjectCode}' already exists"
            );

        var subject = Domain.Entities.Subject.Create(
            request.SubjectCode,
            request.Name,
            request.Description,
            request.Grade,
            request.ImageUrl
        );

        await _subjectRepository.AddAsync(subject, cancellationToken);
        await _subjectRepository.SaveChangesAsync(cancellationToken);

        return new CreateSubjectResponse(
            subject.Id,
            subject.SubjectCode,
            subject.Name,
            subject.Description,
            subject.Grade,
            subject.ImageUrl
        );
    }
}
