using MediatR;

namespace FrogEdu.Class.Application.Commands.ReinviteStudent;

public sealed record ReinviteStudentCommand(Guid ClassId, Guid StudentId) : IRequest<bool>;
