using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateRole;

public sealed record UpdateRoleCommand(Guid Id, string Name, string Description) : IRequest<Result>;
