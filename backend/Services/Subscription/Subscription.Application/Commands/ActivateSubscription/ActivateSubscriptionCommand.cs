using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.ActivateSubscription;

public sealed record ActivateSubscriptionCommand(Guid SubscriptionId) : IRequest<Result>;
