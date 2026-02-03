using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetUserSubscription;

/// <summary>
/// Query to get a user's active subscription
/// </summary>
public sealed record GetUserSubscriptionQuery(Guid UserId) : IRequest<UserSubscriptionDto?>;
