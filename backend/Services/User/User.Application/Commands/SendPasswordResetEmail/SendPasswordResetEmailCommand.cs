using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.SendPasswordResetEmail;

public sealed record SendPasswordResetEmailCommand(string Email) : IRequest<Result>;
