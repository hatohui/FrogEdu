using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateUser;

/// <summary>
/// Command to create a new user (from Cognito Post-Confirmation webhook)
/// </summary>
public sealed record CreateUserCommand(
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role
) : IRequest<Result<Guid>>;
