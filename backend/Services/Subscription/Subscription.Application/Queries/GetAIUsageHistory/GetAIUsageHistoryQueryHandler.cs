using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAIUsageHistory;

public sealed class GetAIUsageHistoryQueryHandler
    : IRequestHandler<GetAIUsageHistoryQuery, IReadOnlyList<AIUsageDto>>
{
    private readonly IAIUsageRecordRepository _aiUsageRepo;

    public GetAIUsageHistoryQueryHandler(IAIUsageRecordRepository aiUsageRepo)
    {
        _aiUsageRepo = aiUsageRepo;
    }

    public async Task<IReadOnlyList<AIUsageDto>> Handle(
        GetAIUsageHistoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var records = await _aiUsageRepo.GetByUserIdAsync(request.UserId, cancellationToken);

        return records
            .Select(r => new AIUsageDto
            {
                Id = r.Id,
                UserId = r.UserId,
                ActionType = r.ActionType,
                UsedAt = r.UsedAt,
                Metadata = r.Metadata,
            })
            .ToList()
            .AsReadOnly();
    }
}
