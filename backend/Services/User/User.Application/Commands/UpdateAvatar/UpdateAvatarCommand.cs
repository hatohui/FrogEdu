using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateAvatar;

/// <summary>
/// Command to update user avatar URL
/// </summary>
public sealed record UpdateAvatarCommand(string CognitoId, string AvatarUrl) : IRequest<Result>;
