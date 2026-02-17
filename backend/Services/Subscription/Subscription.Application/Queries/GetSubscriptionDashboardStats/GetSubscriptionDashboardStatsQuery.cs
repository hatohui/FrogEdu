using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetSubscriptionDashboardStats;

public sealed record GetSubscriptionDashboardStatsQuery()
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
    IReadOnlyList<SubscriptionStatusItem> StatusDistribution
);
