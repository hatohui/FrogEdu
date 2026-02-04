using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.SuspendSubscription;

public sealed class SuspendSubscriptionCommandHandler
    : IRequestHandler<SuspendSubscriptionCommand, Result>
{
    private readonly IUserSubscriptionRepository _repository;

    public SuspendSubscriptionCommandHandler(IUserSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        SuspendSubscriptionCommand request,
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

        subscription.Suspend();

        _repository.Update(subscription);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
