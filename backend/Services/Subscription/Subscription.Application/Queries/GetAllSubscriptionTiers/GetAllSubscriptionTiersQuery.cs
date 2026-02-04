using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllSubscriptionTiers;

public sealed record GetAllSubscriptionTiersQuery(bool IncludeInactive = false)
    : IRequest<IReadOnlyList<SubscriptionTierDto>>;
