using MediatR;

namespace FrogEdu.Class.Application.Commands.AcceptReinvite;

public sealed record AcceptReinviteCommand(Guid ClassId, Guid StudentId) : IRequest<bool>;
