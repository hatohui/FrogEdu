using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.UpdateSubscriptionTier;

public sealed record UpdateSubscriptionTierCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int DurationInDays,
    string? ImageUrl
) : IRequest<Result>;
