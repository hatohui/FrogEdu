using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.DeleteSubscription;

public sealed class DeleteSubscriptionCommandHandler
    : IRequestHandler<DeleteSubscriptionCommand, Result>
{
    private readonly IUserSubscriptionRepository _repository;

    public DeleteSubscriptionCommandHandler(IUserSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        DeleteSubscriptionCommand request,
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

        _repository.Delete(subscription);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
