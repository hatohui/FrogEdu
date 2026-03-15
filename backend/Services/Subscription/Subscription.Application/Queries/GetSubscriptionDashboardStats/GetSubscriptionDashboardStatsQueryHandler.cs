using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionDashboardStats;

public sealed class GetSubscriptionDashboardStatsQueryHandler(
    IUserSubscriptionRepository subscriptionRepository,
    ITransactionRepository transactionRepository,
    IAIUsageRecordRepository aiUsageRepository,
    ILogger<GetSubscriptionDashboardStatsQueryHandler> logger
) : IRequestHandler<GetSubscriptionDashboardStatsQuery, SubscriptionDashboardStatsResponse>
{
    private readonly IUserSubscriptionRepository _subscriptionRepository = subscriptionRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IAIUsageRecordRepository _aiUsageRepository = aiUsageRepository;
    private readonly ILogger<GetSubscriptionDashboardStatsQueryHandler> _logger = logger;

    private static DateTime GetStartDate(string timeRange)
    {
        var now = DateTime.UtcNow;
        return timeRange switch
        {
            "7d" => now.AddDays(-7).Date,
            "30d" => now.AddDays(-30).Date,
            "90d" => now.AddDays(-90).Date,
            "1y" => now.AddYears(-1).Date,
            "all" => DateTime.MinValue,
            _ => now.AddDays(-30).Date,
        };
    }

    public async Task<SubscriptionDashboardStatsResponse> Handle(
        GetSubscriptionDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        var startDate = GetStartDate(request.TimeRange);
        var subscriptions = await _subscriptionRepository.GetAllAsync(cancellationToken);
        var transactions = await _transactionRepository.GetAllAsync(cancellationToken);

        // Filter by time range
        var filteredSubscriptions =
            startDate == DateTime.MinValue
                ? subscriptions
                : subscriptions.Where(s => s.StartDate >= startDate).ToList();
        var filteredTransactions =
            startDate == DateTime.MinValue
                ? transactions
                : transactions.Where(t => t.CreatedAt >= startDate).ToList();

        // Total revenue from completed transactions in range
        var completedTransactions = filteredTransactions
            .Where(t => t.PaymentStatus == PaymentStatus.Paid)
            .ToList();
        var totalRevenue = completedTransactions.Sum(t => t.Amount.Amount);

        // Subscription status counts (always show current status totals)
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

        // Monthly revenue based on time range
        var now = DateTime.UtcNow;
        var monthCount = request.TimeRange switch
        {
            "7d" => 1,
            "30d" => 3,
            "90d" => 6,
            "1y" => 12,
            _ => 6,
        };
        var monthlyRevenue = new List<MonthlyRevenueItem>();
        for (var i = monthCount - 1; i >= 0; i--)
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

        // AI Usage stats
        var allAIUsage = await _aiUsageRepository.GetAllAsync(cancellationToken);
        var filteredAIUsage =
            startDate == DateTime.MinValue
                ? allAIUsage
                : allAIUsage.Where(r => r.UsedAt >= startDate).ToList();
        var totalAIUsageCount = filteredAIUsage.Count;

        var aiUsageOverTime = filteredAIUsage
            .GroupBy(r => r.UsedAt.Date)
            .OrderBy(g => g.Key)
            .Select(g => new AIUsageSummaryItem(g.Key.ToString("yyyy-MM-dd"), g.Count()))
            .ToList();

        _logger.LogInformation(
            "Retrieved subscription dashboard stats ({TimeRange}): {Total} subscriptions, {Revenue} total revenue, {AIUsage} AI usages",
            request.TimeRange,
            totalSubscriptions,
            totalRevenue,
            totalAIUsageCount
        );

        return new SubscriptionDashboardStatsResponse(
            totalRevenue,
            totalSubscriptions,
            activeCount,
            expiredCount,
            cancelledCount,
            suspendedCount,
            monthlyRevenue,
            statusDistribution,
            totalAIUsageCount,
            aiUsageOverTime
        );
    }
}
