using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.ActivateSubscriptionTier;

public sealed record ActivateSubscriptionTierCommand(Guid Id) : IRequest<Result>;
