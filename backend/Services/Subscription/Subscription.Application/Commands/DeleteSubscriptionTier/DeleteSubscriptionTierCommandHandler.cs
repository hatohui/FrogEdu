using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeleteSubscriptionTier;

public sealed class DeleteSubscriptionTierCommandHandler
    : IRequestHandler<DeleteSubscriptionTierCommand, Result>
{
    private readonly ISubscriptionTierRepository _repository;

    public DeleteSubscriptionTierCommandHandler(ISubscriptionTierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        DeleteSubscriptionTierCommand request,
        CancellationToken cancellationToken
    )
    {
        var tier = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tier is null)
        {
            return Result.Failure("Subscription tier not found");
        }

        _repository.Delete(tier);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
