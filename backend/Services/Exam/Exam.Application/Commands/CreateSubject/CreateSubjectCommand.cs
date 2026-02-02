using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateSubject;

public sealed record CreateSubjectCommand(
    string SubjectCode,
    string Name,
    string Description,
    int Grade,
    string? ImageUrl
) : IRequest<CreateSubjectResponse>;

public sealed record CreateSubjectResponse(
    Guid Id,
    string SubjectCode,
    string Name,
    string Description,
    int Grade,
    string? ImageUrl
);
