using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetUserSubscription;

/// <summary>
/// Handler for GetUserSubscriptionQuery
/// </summary>
public sealed class GetUserSubscriptionQueryHandler
    : IRequestHandler<GetUserSubscriptionQuery, UserSubscriptionDto?>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionTierRepository _subscriptionTierRepository;

    public GetUserSubscriptionQueryHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionTierRepository subscriptionTierRepository
    )
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionTierRepository = subscriptionTierRepository;
    }

    public async Task<UserSubscriptionDto?> Handle(
        GetUserSubscriptionQuery request,
        CancellationToken cancellationToken
    )
    {
        var subscription = await _userSubscriptionRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        if (subscription is null)
        {
            return null;
        }

        var tier = await _subscriptionTierRepository.GetByIdAsync(
            subscription.SubscriptionTierId,
            cancellationToken
        );

        var expiresAtTimestamp = new DateTimeOffset(subscription.EndDate).ToUnixTimeSeconds();

        return new UserSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            PlanName = tier?.Name ?? "Unknown",
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Status = subscription.Status.ToString(),
            IsActive = subscription.IsActive(),
            IsExpired = subscription.IsExpired(),
            ExpiresAtTimestamp = expiresAtTimestamp,
        };
    }
}
