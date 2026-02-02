using FrogEdu.Shared.Kernel.Primitives;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateRole;

public sealed record CreateRoleCommand(string Name, string Description) : IRequest<Result<Guid>>;
