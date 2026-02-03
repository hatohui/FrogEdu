using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.SubscribeToPro;

/// <summary>
/// Command to subscribe a user to Pro tier (mock - no payment required)
/// </summary>
public sealed record SubscribeToProCommand(Guid UserId) : IRequest<Result<Guid>>;
