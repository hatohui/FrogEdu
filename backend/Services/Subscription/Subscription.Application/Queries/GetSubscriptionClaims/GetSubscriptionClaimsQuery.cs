using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionClaims;

/// <summary>
/// Query to get subscription claims for JWT - used by User Service
/// </summary>
public sealed record GetSubscriptionClaimsQuery(Guid UserId) : IRequest<SubscriptionClaimsDto>;
