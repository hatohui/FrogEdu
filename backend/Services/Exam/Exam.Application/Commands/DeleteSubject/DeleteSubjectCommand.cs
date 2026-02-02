using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteSubject;

public sealed record DeleteSubjectCommand(Guid SubjectId) : IRequest<Unit>;
