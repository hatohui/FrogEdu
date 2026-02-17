using MediatR;

namespace FrogEdu.Class.Application.Commands.RemoveStudent;

public sealed record RemoveStudentCommand(Guid ClassId, Guid StudentId) : IRequest<bool>;
