using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Domain.ValueObjects;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.CreateSubscriptionTier;

public sealed class CreateSubscriptionTierCommandHandler
    : IRequestHandler<CreateSubscriptionTierCommand, Result<Guid>>
{
    private readonly ISubscriptionTierRepository _repository;

    public CreateSubscriptionTierCommandHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(
        CreateSubscriptionTierCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if tier with same name already exists
        var existingTier = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (existingTier is not null)
        {
            return Result<Guid>.Failure(
                $"Subscription tier with name '{request.Name}' already exists"
            );
        }

        // Create Money value object
        var price = Money.Create(request.Price, request.Currency);

        // Create subscription tier entity
        var tier = SubscriptionTier.Create(
            request.Name,
            request.Description,
            price,
            request.DurationInDays,
            request.TargetRole,
            request.ImageUrl
        );

        // Persist
        await _repository.AddAsync(tier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(tier.Id);
    }
}
