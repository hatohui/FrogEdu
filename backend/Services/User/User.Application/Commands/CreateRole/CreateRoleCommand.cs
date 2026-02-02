using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateRole;

public sealed record CreateRoleCommand(string Name, string Description) : IRequest<Result<Guid>>;
