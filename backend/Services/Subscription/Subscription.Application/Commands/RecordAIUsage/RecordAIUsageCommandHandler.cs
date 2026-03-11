using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Subscription.Application.Commands.RecordAIUsage;

public sealed class RecordAIUsageCommandHandler
    : IRequestHandler<RecordAIUsageCommand, Result<Guid>>
{
    private const int FreeMaxAIGenerations = 3;
    private const int ProMaxAIGenerations = 300;
    private readonly IAIUsageRecordRepository _aiUsageRepo;
    private readonly IUserSubscriptionRepository _subscriptionRepo;
    private readonly ILogger<RecordAIUsageCommandHandler> _logger;

    public RecordAIUsageCommandHandler(
        IAIUsageRecordRepository aiUsageRepo,
        IUserSubscriptionRepository subscriptionRepo,
        ILogger<RecordAIUsageCommandHandler> logger
    )
    {
        _aiUsageRepo = aiUsageRepo;
        _subscriptionRepo = subscriptionRepo;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        RecordAIUsageCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check subscription status
        var activeSubscription = await _subscriptionRepo.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken
        );
        var isPaid = activeSubscription is not null && activeSubscription.IsActive();

        if (isPaid)
        {
            // Pro tier: enforce monthly limit
            var startOfMonth = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            );
            var currentUsage = await _aiUsageRepo.GetUsageCountSinceAsync(
                request.UserId,
                startOfMonth,
                "question_generation",
                cancellationToken
            );

            if (currentUsage >= ProMaxAIGenerations)
            {
                _logger.LogWarning(
                    "User {UserId} has reached Pro AI usage limit ({Limit}/month)",
                    request.UserId,
                    ProMaxAIGenerations
                );
                return Result<Guid>.Failure(
                    $"Pro tier monthly AI usage limit reached ({ProMaxAIGenerations} generations/month). Limit resets at the start of next month."
                );
            }
        }
        else
        {
            // Free tier: enforce total limit
            var currentUsage = await _aiUsageRepo.GetUsageCountAsync(
                request.UserId,
                "question_generation",
                cancellationToken
            );

            if (currentUsage >= FreeMaxAIGenerations)
            {
                _logger.LogWarning(
                    "User {UserId} has reached AI usage limit ({Limit})",
                    request.UserId,
                    FreeMaxAIGenerations
                );
                return Result<Guid>.Failure(
                    $"Free tier AI usage limit reached ({FreeMaxAIGenerations} generations). Upgrade to Pro for 300 AI generations per month."
                );
            }
        }

        var record = AIUsageRecord.Create(request.UserId, request.ActionType, request.Metadata);
        await _aiUsageRepo.AddAsync(record, cancellationToken);
        await _aiUsageRepo.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Recorded AI usage for user {UserId}: {ActionType}",
            request.UserId,
            request.ActionType
        );
        return Result<Guid>.Success(record.Id);
    }
}
