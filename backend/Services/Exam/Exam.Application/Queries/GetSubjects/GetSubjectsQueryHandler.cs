using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetSubjects;

public sealed class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, GetSubjectsResponse>
{
    private readonly ISubjectRepository _subjectRepository;

    public GetSubjectsQueryHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<GetSubjectsResponse> Handle(
        GetSubjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Domain.Entities.Subject> subjects;

        if (request.Grade.HasValue)
        {
            subjects = await _subjectRepository.GetByGradeAsync(
                request.Grade.Value,
                cancellationToken
            );
        }
        else
        {
            subjects = await _subjectRepository.GetAllAsync(cancellationToken);
        }

        var subjectDtos = subjects
            .Select(s => new SubjectDto(
                s.Id,
                s.SubjectCode,
                s.Name,
                s.Description,
                s.ImageUrl,
                s.Grade
            ))
            .ToList();

        return new GetSubjectsResponse(subjectDtos);
    }
}
