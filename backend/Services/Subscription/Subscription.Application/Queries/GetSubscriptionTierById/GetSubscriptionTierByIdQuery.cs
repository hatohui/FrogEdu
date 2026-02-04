using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionTierById;

public sealed record GetSubscriptionTierByIdQuery(Guid Id) : IRequest<SubscriptionTierDto?>;
