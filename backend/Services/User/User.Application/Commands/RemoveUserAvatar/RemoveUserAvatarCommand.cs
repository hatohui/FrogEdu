using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.RemoveUserAvatar;

public sealed record RemoveUserAvatarCommand(string CognitoId) : IRequest<Result>;
