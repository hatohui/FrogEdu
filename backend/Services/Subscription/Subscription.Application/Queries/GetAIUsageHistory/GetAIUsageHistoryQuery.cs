using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAIUsageHistory;

public sealed record GetAIUsageHistoryQuery(Guid UserId) : IRequest<IReadOnlyList<AIUsageDto>>;
