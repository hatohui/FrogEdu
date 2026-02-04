using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeactivateSubscriptionTier;

public sealed record DeactivateSubscriptionTierCommand(Guid Id) : IRequest<Result>;
