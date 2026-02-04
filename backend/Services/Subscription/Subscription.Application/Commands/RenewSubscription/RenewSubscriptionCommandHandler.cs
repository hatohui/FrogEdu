using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.RenewSubscription;

public sealed class RenewSubscriptionCommandHandler
    : IRequestHandler<RenewSubscriptionCommand, Result>
{
    private readonly IUserSubscriptionRepository _repository;

    public RenewSubscriptionCommandHandler(IUserSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        RenewSubscriptionCommand request,
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
            subscription.Renew(request.NewEndDate);

            _repository.Update(subscription);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
