using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : IRequest<Result>;
