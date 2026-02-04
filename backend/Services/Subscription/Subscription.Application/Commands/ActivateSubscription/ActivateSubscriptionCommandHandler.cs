using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.ActivateSubscription;

public sealed class ActivateSubscriptionCommandHandler
    : IRequestHandler<ActivateSubscriptionCommand, Result>
{
    private readonly IUserSubscriptionRepository _repository;

    public ActivateSubscriptionCommandHandler(IUserSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        ActivateSubscriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        var subscription = await _repository.GetByIdAsync(
            request.SubscriptionId,
            cancellationToken
        );
        if (subscription is null)
        {
            return Result.Failure("Subscription not found");
        }

        try
        {
            subscription.Activate();

            _repository.Update(subscription);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
