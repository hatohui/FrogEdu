using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.RegenerateInviteCode;

/// <summary>
/// Command to regenerate invite code for a class
/// </summary>
public sealed record RegenerateInviteCodeCommand(
    Guid ClassId,
    Guid TeacherId,
    int ExpiresInDays = 7
) : IRequest<Result<string>>;
