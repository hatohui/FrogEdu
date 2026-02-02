using FrogEdu.Shared.Kernel.Primitives;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : IRequest<Result>;
