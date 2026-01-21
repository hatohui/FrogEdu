using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.JoinClass;

/// <summary>
/// Command to join a class via invite code
/// </summary>
public sealed record JoinClassCommand(string InviteCode, Guid StudentId) : IRequest<Result<Guid>>;
