using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.SuspendSubscription;

public sealed record SuspendSubscriptionCommand(Guid SubscriptionId) : IRequest<Result>;
