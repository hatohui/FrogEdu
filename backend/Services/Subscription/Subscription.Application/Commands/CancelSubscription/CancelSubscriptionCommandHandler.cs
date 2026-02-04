using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.CancelSubscription;

/// <summary>
/// Handler for cancelling a user's active subscription
/// </summary>
public sealed class CancelSubscriptionCommandHandler
    : IRequestHandler<CancelSubscriptionCommand, Result<Guid>>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;

    public CancelSubscriptionCommandHandler(IUserSubscriptionRepository userSubscriptionRepository)
    {
        _userSubscriptionRepository = userSubscriptionRepository;
    }

    public async Task<Result<Guid>> Handle(
        CancelSubscriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        // Get user's active subscription
        var subscription = await _userSubscriptionRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        if (subscription is null)
        {
            return Result<Guid>.Failure("No active subscription found for this user");
        }

        // Check if already cancelled
        if (!subscription.IsActive())
        {
            return Result<Guid>.Failure("Subscription is already cancelled or expired");
        }

        // Cancel the subscription
        subscription.Cancel();

        _userSubscriptionRepository.Update(subscription);
        await _userSubscriptionRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscription.Id);
    }
}
