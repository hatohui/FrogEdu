using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionTiers;

/// <summary>
/// Handler for GetSubscriptionTiersQuery
/// </summary>
public sealed class GetSubscriptionTiersQueryHandler
    : IRequestHandler<GetSubscriptionTiersQuery, IReadOnlyList<SubscriptionTierDto>>
{
    private readonly ISubscriptionTierRepository _subscriptionTierRepository;

    public GetSubscriptionTiersQueryHandler(ISubscriptionTierRepository subscriptionTierRepository)
    {
        _subscriptionTierRepository = subscriptionTierRepository;
    }

    public async Task<IReadOnlyList<SubscriptionTierDto>> Handle(
        GetSubscriptionTiersQuery request,
        CancellationToken cancellationToken
    )
    {
        var tiers = await _subscriptionTierRepository.GetAllActiveAsync(cancellationToken);

        return tiers
            .Select(t => new SubscriptionTierDto
            {
                Id = t.Id,
                Name = t.Name,
                ImageUrl = t.ImageUrl,
                Description = t.Description,
                Price = t.Price.Amount,
                Currency = t.Price.Currency,
                DurationInDays = t.DurationInDays,
                TargetRole = t.TargetRole,
                IsActive = t.IsActive,
            })
            .ToList();
    }
}
