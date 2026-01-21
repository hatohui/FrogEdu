using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateProfile;

/// <summary>
/// Command to update user profile
/// </summary>
public sealed record UpdateProfileCommand(string CognitoId, string FirstName, string LastName)
    : IRequest<Result>;
