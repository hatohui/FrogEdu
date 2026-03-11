using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Subscription.Application.Queries.GetAIUsageLimit;

public sealed class GetAIUsageLimitQueryHandler
    : IRequestHandler<GetAIUsageLimitQuery, AIUsageLimitDto>
{
    private const int FreeMaxAIGenerations = 3;
    private const int ProMaxAIGenerations = 300;
    private readonly IAIUsageRecordRepository _aiUsageRepo;
    private readonly IUserSubscriptionRepository _subscriptionRepo;
    private readonly ILogger<GetAIUsageLimitQueryHandler> _logger;

    public GetAIUsageLimitQueryHandler(
        IAIUsageRecordRepository aiUsageRepo,
        IUserSubscriptionRepository subscriptionRepo,
        ILogger<GetAIUsageLimitQueryHandler> logger
    )
    {
        _aiUsageRepo = aiUsageRepo;
        _subscriptionRepo = subscriptionRepo;
        _logger = logger;
    }

    public async Task<AIUsageLimitDto> Handle(
        GetAIUsageLimitQuery request,
        CancellationToken cancellationToken
    )
    {
        var activeSubscription = await _subscriptionRepo.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );
        var isPaid = activeSubscription is not null && activeSubscription.IsActive();

        if (isPaid)
        {
            var startOfMonth = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            );
            var usedCount = await _aiUsageRepo.GetUsageCountSinceAsync(
                request.UserId,
                startOfMonth,
                "question_generation",
                cancellationToken
            );
            var remaining = Math.Max(0, ProMaxAIGenerations - usedCount);

            return new AIUsageLimitDto
            {
                UserId = request.UserId,
                Plan = "Pro",
                UsedCount = usedCount,
                MaxAllowed = ProMaxAIGenerations,
                Remaining = remaining,
                CanUseAI = remaining > 0,
                IsUnlimited = false,
            };
        }

        var freeUsedCount = await _aiUsageRepo.GetUsageCountAsync(
            request.UserId,
            "question_generation",
            cancellationToken
        );
        var freeRemaining = Math.Max(0, FreeMaxAIGenerations - freeUsedCount);
        return new AIUsageLimitDto
        {
            UserId = request.UserId,
            Plan = "Free",
            UsedCount = freeUsedCount,
            MaxAllowed = FreeMaxAIGenerations,
            Remaining = freeRemaining,
            CanUseAI = freeRemaining > 0,
            IsUnlimited = false,
        };
    }
}
