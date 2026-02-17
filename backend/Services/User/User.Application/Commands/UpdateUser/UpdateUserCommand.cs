using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string? FirstName,
    string? LastName,
    Guid? RoleId
) : IRequest<Result>;
