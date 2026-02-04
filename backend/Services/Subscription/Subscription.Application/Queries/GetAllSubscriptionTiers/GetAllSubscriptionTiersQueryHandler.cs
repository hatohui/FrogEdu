using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllSubscriptionTiers;

public sealed class GetAllSubscriptionTiersQueryHandler
    : IRequestHandler<GetAllSubscriptionTiersQuery, IReadOnlyList<SubscriptionTierDto>>
{
    private readonly ISubscriptionTierRepository _repository;

    public GetAllSubscriptionTiersQueryHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SubscriptionTierDto>> Handle(
        GetAllSubscriptionTiersQuery request,
        CancellationToken cancellationToken
    )
    {
        var tiers = request.IncludeInactive
            ? await _repository.GetAllAsync(cancellationToken)
            : await _repository.GetAllActiveAsync(cancellationToken);

        return tiers
            .Select(tier => new SubscriptionTierDto
            {
                Id = tier.Id,
                Name = tier.Name,
                ImageUrl = tier.ImageUrl,
                Description = tier.Description,
                Price = tier.Price.Amount,
                Currency = tier.Price.Currency,
                DurationInDays = tier.DurationInDays,
                TargetRole = tier.TargetRole,
                IsActive = tier.IsActive,
            })
            .ToList();
    }
}
