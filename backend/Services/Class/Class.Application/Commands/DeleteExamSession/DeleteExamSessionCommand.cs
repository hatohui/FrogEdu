using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.DeleteExamSession;

public sealed record DeleteExamSessionCommand(Guid SessionId, string UserId) : IRequest<Result>;
