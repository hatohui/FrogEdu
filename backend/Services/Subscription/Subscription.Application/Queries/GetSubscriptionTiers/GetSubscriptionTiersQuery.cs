using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionTiers;

/// <summary>
/// Query to get all active subscription tiers
/// </summary>
public sealed record GetSubscriptionTiersQuery : IRequest<IReadOnlyList<SubscriptionTierDto>>;
