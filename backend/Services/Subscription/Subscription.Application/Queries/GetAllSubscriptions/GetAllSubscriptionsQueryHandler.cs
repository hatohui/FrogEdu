using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllSubscriptions;

public sealed class GetAllSubscriptionsQueryHandler
    : IRequestHandler<GetAllSubscriptionsQuery, IReadOnlyList<UserSubscriptionDto>>
{
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionTierRepository _tierRepository;

    public GetAllSubscriptionsQueryHandler(
        IUserSubscriptionRepository subscriptionRepository,
        ISubscriptionTierRepository tierRepository
    )
    {
        _subscriptionRepository = subscriptionRepository;
        _tierRepository = tierRepository;
    }

    public async Task<IReadOnlyList<UserSubscriptionDto>> Handle(
        GetAllSubscriptionsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Domain.Entities.UserSubscription> subscriptions;

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (Enum.TryParse<SubscriptionStatus>(request.Status, true, out var status))
            {
                subscriptions = await _subscriptionRepository.GetByStatusAsync(
                    status,
                    cancellationToken
                );
            }
            else
            {
                return new List<UserSubscriptionDto>();
            }
        }
        else
        {
            subscriptions = await _subscriptionRepository.GetAllAsync(cancellationToken);
        }

        var result = new List<UserSubscriptionDto>();

        foreach (var subscription in subscriptions)
        {
            var tier = await _tierRepository.GetByIdAsync(
                subscription.SubscriptionTierId,
                cancellationToken
            );

            result.Add(
                new UserSubscriptionDto
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
                    ExpiresAtTimestamp = new DateTimeOffset(
                        subscription.EndDate
                    ).ToUnixTimeSeconds(),
                }
            );
        }

        return result;
    }
}
