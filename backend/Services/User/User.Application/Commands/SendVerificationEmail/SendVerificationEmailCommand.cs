using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.SendVerificationEmail;

public sealed record SendVerificationEmailCommand(Guid UserId) : IRequest<Result>;
