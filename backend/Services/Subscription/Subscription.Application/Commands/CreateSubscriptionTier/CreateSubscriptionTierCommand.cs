using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.CreateSubscriptionTier;

public sealed record CreateSubscriptionTierCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int DurationInDays,
    string TargetRole,
    string? ImageUrl
) : IRequest<Result<Guid>>;
