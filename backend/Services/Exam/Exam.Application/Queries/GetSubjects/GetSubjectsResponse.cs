namespace FrogEdu.Exam.Application.Queries.GetSubjects;

public sealed record SubjectDto(
    Guid Id,
    string SubjectCode,
    string Name,
    string Description,
    string ImageUrl,
    int Grade
);

public sealed record GetSubjectsResponse(List<SubjectDto> Subjects);
