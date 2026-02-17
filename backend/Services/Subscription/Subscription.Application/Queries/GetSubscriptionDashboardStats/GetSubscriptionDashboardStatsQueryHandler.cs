using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionDashboardStats;

public sealed class GetSubscriptionDashboardStatsQueryHandler(
    IUserSubscriptionRepository subscriptionRepository,
    ITransactionRepository transactionRepository,
    ILogger<GetSubscriptionDashboardStatsQueryHandler> logger
) : IRequestHandler<GetSubscriptionDashboardStatsQuery, SubscriptionDashboardStatsResponse>
{
    private readonly IUserSubscriptionRepository _subscriptionRepository = subscriptionRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ILogger<GetSubscriptionDashboardStatsQueryHandler> _logger = logger;

    public async Task<SubscriptionDashboardStatsResponse> Handle(
        GetSubscriptionDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync(cancellationToken);
        var transactions = await _transactionRepository.GetAllAsync(cancellationToken);

        // Total revenue from completed transactions
        var completedTransactions = transactions
            .Where(t => t.PaymentStatus == PaymentStatus.Paid)
            .ToList();
        var totalRevenue = completedTransactions.Sum(t => t.Amount.Amount);

        // Subscription status counts
        var totalSubscriptions = subscriptions.Count;
        var activeCount = subscriptions.Count(s => s.Status == SubscriptionStatus.Active);
        var expiredCount = subscriptions.Count(s => s.Status == SubscriptionStatus.Expired);
        var cancelledCount = subscriptions.Count(s => s.Status == SubscriptionStatus.Cancelled);
        var suspendedCount = subscriptions.Count(s => s.Status == SubscriptionStatus.Suspended);

        // Status distribution
        var statusDistribution = new List<SubscriptionStatusItem>
        {
            new(
                "Active",
                activeCount,
                totalSubscriptions > 0
                    ? Math.Round((double)activeCount / totalSubscriptions * 100, 1)
                    : 0
            ),
            new(
                "Expired",
                expiredCount,
                totalSubscriptions > 0
                    ? Math.Round((double)expiredCount / totalSubscriptions * 100, 1)
                    : 0
            ),
            new(
                "Cancelled",
                cancelledCount,
                totalSubscriptions > 0
                    ? Math.Round((double)cancelledCount / totalSubscriptions * 100, 1)
                    : 0
            ),
            new(
                "Suspended",
                suspendedCount,
                totalSubscriptions > 0
                    ? Math.Round((double)suspendedCount / totalSubscriptions * 100, 1)
                    : 0
            ),
        };

        // Monthly revenue (last 6 months)
        var now = DateTime.UtcNow;
        var monthlyRevenue = new List<MonthlyRevenueItem>();
        for (var i = 5; i >= 0; i--)
        {
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthLabel = monthStart.ToString("MMM yyyy");

            var monthTransactions = completedTransactions
                .Where(t => t.CreatedAt >= monthStart && t.CreatedAt < monthEnd)
                .ToList();

            monthlyRevenue.Add(
                new MonthlyRevenueItem(
                    monthLabel,
                    monthTransactions.Sum(t => t.Amount.Amount),
                    monthTransactions.Count
                )
            );
        }

        _logger.LogInformation(
            "Retrieved subscription dashboard stats: {Total} subscriptions, {Revenue} total revenue",
            totalSubscriptions,
            totalRevenue
        );

        return new SubscriptionDashboardStatsResponse(
            totalRevenue,
            totalSubscriptions,
            activeCount,
            expiredCount,
            cancelledCount,
            suspendedCount,
            monthlyRevenue,
            statusDistribution
        );
    }
}
