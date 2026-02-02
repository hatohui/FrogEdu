using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteSubject;

public sealed class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, Unit>
{
    private readonly ISubjectRepository _subjectRepository;

    public DeleteSubjectCommandHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<Unit> Handle(
        DeleteSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        _subjectRepository.Delete(subject);
        await _subjectRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
