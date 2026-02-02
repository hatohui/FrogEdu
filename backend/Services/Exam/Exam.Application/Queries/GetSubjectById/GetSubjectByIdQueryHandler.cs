using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetSubjectById;

public sealed class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto?>
{
    private readonly ISubjectRepository _subjectRepository;

    public GetSubjectByIdQueryHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<SubjectDto?> Handle(
        GetSubjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);

        if (subject is null)
            return null;

        return new SubjectDto(
            subject.Id,
            subject.SubjectCode,
            subject.Name,
            subject.Description,
            subject.ImageUrl,
            subject.Grade
        );
    }
}
