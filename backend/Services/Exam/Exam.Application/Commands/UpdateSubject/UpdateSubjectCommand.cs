using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateSubject;

public sealed record UpdateSubjectCommand(
    Guid SubjectId,
    string Name,
    string Description,
    string? ImageUrl
) : IRequest<Unit>;
