using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : IRequest<Result>;
