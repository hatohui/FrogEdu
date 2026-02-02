using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Result>;
