using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateSubject;

public sealed class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Unit>
{
    private readonly ISubjectRepository _subjectRepository;

    public UpdateSubjectCommandHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<Unit> Handle(
        UpdateSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        subject.UpdateProfile(request.Name, request.Description, request.ImageUrl);

        _subjectRepository.Update(subject);
        await _subjectRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
