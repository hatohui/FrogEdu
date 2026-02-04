using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.ActivateSubscriptionTier;

public sealed class ActivateSubscriptionTierCommandHandler
    : IRequestHandler<ActivateSubscriptionTierCommand, Result>
{
    private readonly ISubscriptionTierRepository _repository;

    public ActivateSubscriptionTierCommandHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        ActivateSubscriptionTierCommand request,
        CancellationToken cancellationToken
    )
    {
        var tier = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tier is null)
        {
            return Result.Failure("Subscription tier not found");
        }

        tier.Activate();

        _repository.Update(tier);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
