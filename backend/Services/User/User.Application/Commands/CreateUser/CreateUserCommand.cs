using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateUser;

public sealed record CreateUserCommand(
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role
) : IRequest<Result<Guid>>;
