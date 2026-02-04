using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.RenewSubscription;

public sealed record RenewSubscriptionCommand(Guid SubscriptionId, DateTime NewEndDate)
    : IRequest<Result>;
