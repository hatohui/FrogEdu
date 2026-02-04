using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeleteSubscription;

public sealed record DeleteSubscriptionCommand(Guid SubscriptionId) : IRequest<Result>;
