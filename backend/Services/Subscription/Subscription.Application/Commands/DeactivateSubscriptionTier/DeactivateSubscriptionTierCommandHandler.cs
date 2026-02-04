using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeactivateSubscriptionTier;

public sealed class DeactivateSubscriptionTierCommandHandler
    : IRequestHandler<DeactivateSubscriptionTierCommand, Result>
{
    private readonly ISubscriptionTierRepository _repository;

    public DeactivateSubscriptionTierCommandHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        DeactivateSubscriptionTierCommand request,
        CancellationToken cancellationToken
    )
    {
        var tier = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tier is null)
        {
            return Result.Failure("Subscription tier not found");
        }

        tier.Deactivate();

        _repository.Update(tier);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
