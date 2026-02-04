using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllSubscriptions;

public sealed record GetAllSubscriptionsQuery(string? Status = null)
    : IRequest<IReadOnlyList<UserSubscriptionDto>>;
