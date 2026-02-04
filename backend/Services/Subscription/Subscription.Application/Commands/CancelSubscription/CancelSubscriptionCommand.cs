using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.CancelSubscription;

/// <summary>
/// Command to cancel an active subscription for a user
/// </summary>
public sealed record CancelSubscriptionCommand(Guid UserId) : IRequest<Result<Guid>>;
