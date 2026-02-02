using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(string Token) : IRequest<Result>;
