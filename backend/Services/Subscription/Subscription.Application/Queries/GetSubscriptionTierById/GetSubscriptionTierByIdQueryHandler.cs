using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionTierById;

public sealed class GetSubscriptionTierByIdQueryHandler
    : IRequestHandler<GetSubscriptionTierByIdQuery, SubscriptionTierDto?>
{
    private readonly ISubscriptionTierRepository _repository;

    public GetSubscriptionTierByIdQueryHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<SubscriptionTierDto?> Handle(
        GetSubscriptionTierByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var tier = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (tier is null)
            return null;

        return new SubscriptionTierDto
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
        };
    }
}
