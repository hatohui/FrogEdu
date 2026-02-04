using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetUserTransactions;

/// <summary>
/// Handler for GetUserTransactionsQuery
/// </summary>
public sealed class GetUserTransactionsQueryHandler
    : IRequestHandler<GetUserTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionTierRepository _subscriptionTierRepository;

    public GetUserTransactionsQueryHandler(
        ITransactionRepository transactionRepository,
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionTierRepository subscriptionTierRepository
    )
    {
        _transactionRepository = transactionRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionTierRepository = subscriptionTierRepository;
    }

    public async Task<IReadOnlyList<TransactionDto>> Handle(
        GetUserTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await _transactionRepository.GetByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        if (transactions.Count == 0)
        {
            return Array.Empty<TransactionDto>();
        }

        // Get all subscriptions to map tier names
        var subscriptions = await _userSubscriptionRepository.GetByUserIdAsync(
            request.UserId,
            cancellationToken
        );

        // Get tier IDs from subscriptions
        var tierIds = subscriptions.Select(s => s.SubscriptionTierId).Distinct().ToList();
        var tierNames = new Dictionary<Guid, string>();

        foreach (var tierId in tierIds)
        {
            var tier = await _subscriptionTierRepository.GetByIdAsync(tierId, cancellationToken);
            if (tier != null)
            {
                tierNames[tierId] = tier.Name;
            }
        }

        // Map subscription ID to tier name
        var subscriptionTierMap = subscriptions.ToDictionary(
            s => s.Id,
            s => tierNames.GetValueOrDefault(s.SubscriptionTierId, "Unknown")
        );

        return transactions
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                TransactionCode = t.TransactionCode,
                Amount = t.Amount.Amount,
                Currency = t.Amount.Currency,
                PaymentProvider = t.PaymentProvider.ToString(),
                PaymentStatus = t.PaymentStatus.ToString(),
                ProviderTransactionId = t.ProviderTransactionId,
                CreatedAt = t.CreatedAt,
                UserSubscriptionId = t.UserSubscriptionId,
                SubscriptionPlanName = subscriptionTierMap.GetValueOrDefault(
                    t.UserSubscriptionId,
                    "Unknown"
                ),
            })
            .ToList();
    }
}
