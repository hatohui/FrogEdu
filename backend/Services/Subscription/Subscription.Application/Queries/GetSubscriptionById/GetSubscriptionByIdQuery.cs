using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionById;

public sealed record GetSubscriptionByIdQuery(Guid Id) : IRequest<UserSubscriptionDto?>;
