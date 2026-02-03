using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetUserWithSubscription;

/// <summary>
/// Handler for GetUserWithSubscriptionQuery
/// </summary>
public sealed class GetUserWithSubscriptionQueryHandler
    : IRequestHandler<GetUserWithSubscriptionQuery, UserWithSubscriptionDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<GetUserWithSubscriptionQueryHandler> _logger;

    public GetUserWithSubscriptionQueryHandler(
        IUserRepository userRepository,
        ISubscriptionService subscriptionService,
        ILogger<GetUserWithSubscriptionQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<UserWithSubscriptionDto?> Handle(
        GetUserWithSubscriptionQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found for CognitoId: {CognitoId}", request.CognitoId);
            return null;
        }

        // Fetch subscription claims from Subscription service
        var subscriptionClaims = await _subscriptionService.GetSubscriptionClaimsAsync(
            user.Id,
            cancellationToken
        );

        _logger.LogInformation(
            "Retrieved user {UserId} with subscription plan: {Plan}",
            user.Id,
            subscriptionClaims.Plan
        );

        return new UserWithSubscriptionDto
        {
            Id = user.Id,
            CognitoId = user.CognitoId.Value,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RoleId = user.RoleId,
            AvatarUrl = user.AvatarUrl,
            IsEmailVerified = user.IsEmailVerified,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Subscription = new SubscriptionInfoDto
            {
                Plan = subscriptionClaims.Plan,
                ExpiresAt = subscriptionClaims.ExpiresAt,
                HasActiveSubscription = subscriptionClaims.HasActiveSubscription,
            },
        };
    }
}
