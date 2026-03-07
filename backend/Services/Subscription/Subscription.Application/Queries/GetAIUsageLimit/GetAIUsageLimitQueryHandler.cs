using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Subscription.Application.Queries.GetAIUsageLimit;

public sealed class GetAIUsageLimitQueryHandler : IRequestHandler<GetAIUsageLimitQuery, AIUsageLimitDto>
{
    private const int FreeMaxAIGenerations = 3;
    private readonly IAIUsageRecordRepository _aiUsageRepo;
    private readonly IUserSubscriptionRepository _subscriptionRepo;
    private readonly ILogger<GetAIUsageLimitQueryHandler> _logger;

    public GetAIUsageLimitQueryHandler(
        IAIUsageRecordRepository aiUsageRepo,
        IUserSubscriptionRepository subscriptionRepo,
        ILogger<GetAIUsageLimitQueryHandler> logger)
    {
        _aiUsageRepo = aiUsageRepo;
        _subscriptionRepo = subscriptionRepo;
        _logger = logger;
    }

    public async Task<AIUsageLimitDto> Handle(GetAIUsageLimitQuery request, CancellationToken cancellationToken)
    {
        var activeSubscription = await _subscriptionRepo.GetActiveByUserIdAsync(request.UserId, cancellationToken);
        var isPaid = activeSubscription is not null && activeSubscription.IsActive();

        var usedCount = await _aiUsageRepo.GetUsageCountAsync(request.UserId, "question_generation", cancellationToken);

        if (isPaid)
        {
            return new AIUsageLimitDto
            {
                UserId = request.UserId,
                Plan = "Pro",
                UsedCount = usedCount,
                MaxAllowed = null,
                Remaining = int.MaxValue,
                CanUseAI = true,
                IsUnlimited = true,
            };
        }

        var remaining = Math.Max(0, FreeMaxAIGenerations - usedCount);
        return new AIUsageLimitDto
        {
            UserId = request.UserId,
            Plan = "Free",
            UsedCount = usedCount,
            MaxAllowed = FreeMaxAIGenerations,
            Remaining = remaining,
            CanUseAI = remaining > 0,
            IsUnlimited = false,
        };
    }
}
