using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.SubscribeToPro;

/// <summary>
/// Handler for subscribing a user to Pro tier (mock implementation)
/// </summary>
public sealed class SubscribeToProCommandHandler
    : IRequestHandler<SubscribeToProCommand, Result<Guid>>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionTierRepository _subscriptionTierRepository;

    public SubscribeToProCommandHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionTierRepository subscriptionTierRepository
    )
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionTierRepository = subscriptionTierRepository;
    }

    public async Task<Result<Guid>> Handle(
        SubscribeToProCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if user already has an active subscription
        var existingSubscription = await _userSubscriptionRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        if (existingSubscription is not null && existingSubscription.IsActive())
        {
            return Result<Guid>.Failure("User already has an active subscription");
        }

        // Get the Pro tier
        var proTier = await _subscriptionTierRepository.GetByNameAsync("Pro", cancellationToken);

        if (proTier is null)
        {
            return Result<Guid>.Failure("Pro subscription tier not found");
        }

        // Create subscription (mock - immediate activation)
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(proTier.DurationInDays);

        var subscription = UserSubscription.Create(request.UserId, proTier.Id, startDate, endDate);

        await _userSubscriptionRepository.AddAsync(subscription, cancellationToken);
        await _userSubscriptionRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscription.Id);
    }
}
