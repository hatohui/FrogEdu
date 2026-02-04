using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using FrogEdu.Subscription.Domain.ValueObjects;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.UpdateSubscriptionTier;

public sealed class UpdateSubscriptionTierCommandHandler
    : IRequestHandler<UpdateSubscriptionTierCommand, Result>
{
    private readonly ISubscriptionTierRepository _repository;

    public UpdateSubscriptionTierCommandHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        UpdateSubscriptionTierCommand request,
        CancellationToken cancellationToken
    )
    {
        // Get existing tier
        var tier = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tier is null)
        {
            return Result.Failure("Subscription tier not found");
        }

        // Check if name is being changed and if new name already exists
        if (tier.Name != request.Name)
        {
            var existingTier = await _repository.GetByNameAsync(request.Name, cancellationToken);
            if (existingTier is not null && existingTier.Id != request.Id)
            {
                return Result.Failure(
                    $"Subscription tier with name '{request.Name}' already exists"
                );
            }
        }

        // Create Money value object
        var price = Money.Create(request.Price, request.Currency);

        // Update entity
        tier.Update(
            request.Name,
            request.Description,
            price,
            request.DurationInDays,
            request.ImageUrl
        );

        // Persist
        _repository.Update(tier);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
