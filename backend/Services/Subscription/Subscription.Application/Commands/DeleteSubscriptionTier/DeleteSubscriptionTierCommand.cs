using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeleteSubscriptionTier;

public sealed record DeleteSubscriptionTierCommand(Guid Id) : IRequest<Result>;
