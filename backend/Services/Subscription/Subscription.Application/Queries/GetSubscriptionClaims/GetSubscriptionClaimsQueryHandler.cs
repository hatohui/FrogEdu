using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionClaims;

/// <summary>
/// Handler for GetSubscriptionClaimsQuery - returns lightweight claims for JWT
/// </summary>
public sealed class GetSubscriptionClaimsQueryHandler
    : IRequestHandler<GetSubscriptionClaimsQuery, SubscriptionClaimsDto>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionTierRepository _subscriptionTierRepository;

    public GetSubscriptionClaimsQueryHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionTierRepository subscriptionTierRepository
    )
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionTierRepository = subscriptionTierRepository;
    }

    public async Task<SubscriptionClaimsDto> Handle(
        GetSubscriptionClaimsQuery request,
        CancellationToken cancellationToken
    )
    {
        var subscription = await _userSubscriptionRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        // Return free plan if no active subscription
        if (subscription is null || !subscription.IsActive())
        {
            return new SubscriptionClaimsDto
            {
                UserId = request.UserId,
                Plan = "free",
                ExpiresAt = 0,
                HasActiveSubscription = false,
            };
        }

        var tier = await _subscriptionTierRepository.GetByIdAsync(
            subscription.SubscriptionTierId,
            cancellationToken
        );

        var expiresAt = new DateTimeOffset(subscription.EndDate).ToUnixTimeSeconds();

        return new SubscriptionClaimsDto
        {
            UserId = request.UserId,
            Plan = tier?.Name.ToLowerInvariant() ?? "free",
            ExpiresAt = expiresAt,
            HasActiveSubscription = true,
        };
    }
}
