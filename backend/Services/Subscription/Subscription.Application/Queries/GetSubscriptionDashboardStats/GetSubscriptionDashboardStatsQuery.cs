using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionDashboardStats;

/// <summary>
/// TimeRange: "7d" | "30d" | "90d" | "1y" | "all"
/// </summary>
public sealed record GetSubscriptionDashboardStatsQuery(string TimeRange = "30d")
    : IRequest<SubscriptionDashboardStatsResponse>;

/// <summary>
/// Revenue data point for chart
/// </summary>
public sealed record MonthlyRevenueItem(string Month, decimal Revenue, int TransactionCount);

/// <summary>
/// Subscription status distribution for pie chart
/// </summary>
public sealed record SubscriptionStatusItem(string Status, int Count, double Percentage);

/// <summary>
/// AI usage summary for dashboard
/// </summary>
public sealed record AIUsageSummaryItem(string Date, int Count);

/// <summary>
/// Combined subscription dashboard statistics
/// </summary>
public sealed record SubscriptionDashboardStatsResponse(
    decimal TotalRevenue,
    int TotalSubscriptions,
    int ActiveSubscriptions,
    int ExpiredSubscriptions,
    int CancelledSubscriptions,
    int SuspendedSubscriptions,
    IReadOnlyList<MonthlyRevenueItem> MonthlyRevenue,
    IReadOnlyList<SubscriptionStatusItem> StatusDistribution,
    int TotalAIUsageCount = 0,
    IReadOnlyList<AIUsageSummaryItem>? AIUsageOverTime = null
);
