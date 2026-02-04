using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionById;

public sealed class GetSubscriptionByIdQueryHandler
    : IRequestHandler<GetSubscriptionByIdQuery, UserSubscriptionDto?>
{
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionTierRepository _tierRepository;

    public GetSubscriptionByIdQueryHandler(
        IUserSubscriptionRepository subscriptionRepository,
        ISubscriptionTierRepository tierRepository
    )
    {
        _subscriptionRepository = subscriptionRepository;
        _tierRepository = tierRepository;
    }

    public async Task<UserSubscriptionDto?> Handle(
        GetSubscriptionByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(
            request.Id,
            cancellationToken
        );

        if (subscription is null)
            return null;

        var tier = await _tierRepository.GetByIdAsync(
            subscription.SubscriptionTierId,
            cancellationToken
        );

        return new UserSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SubscriptionTierId = subscription.SubscriptionTierId,
            PlanName = tier?.Name ?? "Unknown",
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Status = subscription.Status.ToString(),
            IsActive = subscription.IsActive(),
            IsExpired = subscription.IsExpired(),
            ExpiresAtTimestamp = new DateTimeOffset(subscription.EndDate).ToUnixTimeSeconds(),
        };
    }
}
