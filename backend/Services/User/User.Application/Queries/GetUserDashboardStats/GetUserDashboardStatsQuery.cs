using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserDashboardStats;

public sealed record GetUserDashboardStatsQuery() : IRequest<UserDashboardStatsResponse>;

/// <summary>
/// Daily user creation count for growth chart
/// </summary>
public sealed record DailyUserCount(string Date, int Count);

/// <summary>
/// Role distribution for pie chart
/// </summary>
public sealed record RoleDistributionItem(string Role, int Count, double Percentage);

/// <summary>
/// Verification status for chart
/// </summary>
public sealed record VerificationStatusItem(string Status, int Count, double Percentage);

/// <summary>
/// Combined dashboard statistics response
/// </summary>
public sealed record UserDashboardStatsResponse(
    IReadOnlyList<DailyUserCount> UserGrowthLast30Days,
    IReadOnlyList<RoleDistributionItem> RoleDistribution,
    IReadOnlyList<VerificationStatusItem> VerificationStatus
);
