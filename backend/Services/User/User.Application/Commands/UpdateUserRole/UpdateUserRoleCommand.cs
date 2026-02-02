using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateUserRole;

public sealed record UpdateUserRoleCommand(Guid UserId, Guid RoleId) : IRequest<Result>;
