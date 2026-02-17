using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.JoinClass;

public sealed record JoinClassCommand(string InviteCode, string UserId)
    : IRequest<Result<JoinClassResponse>>;
