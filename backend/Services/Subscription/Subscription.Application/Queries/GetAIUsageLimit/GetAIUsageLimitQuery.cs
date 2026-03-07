using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAIUsageLimit;

public sealed record GetAIUsageLimitQuery(Guid UserId) : IRequest<AIUsageLimitDto>;
