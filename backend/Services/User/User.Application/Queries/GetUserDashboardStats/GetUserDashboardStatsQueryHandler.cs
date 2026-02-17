using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetUserDashboardStats;

public sealed class GetUserDashboardStatsQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserDashboardStatsQueryHandler> logger
) : IRequestHandler<GetUserDashboardStatsQuery, UserDashboardStatsResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<GetUserDashboardStatsQueryHandler> _logger = logger;

    public async Task<UserDashboardStatsResponse> Handle(
        GetUserDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var thirtyDaysAgo = now.AddDays(-30).Date;

        // User growth: daily registration counts for the last 30 days
        var growthData = new List<DailyUserCount>();
        for (var day = thirtyDaysAgo; day <= now.Date; day = day.AddDays(1))
        {
            var count = users.Count(u => u.CreatedAt.Date == day);
            growthData.Add(new DailyUserCount(day.ToString("yyyy-MM-dd"), count));
        }

        // Role distribution
        var totalUsers = users.Count;
        var adminCount = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Admin");
        var teacherCount = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Teacher");
        var studentCount = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Student");

        var roleDistribution = new List<RoleDistributionItem>
        {
            new(
                "Admin",
                adminCount,
                totalUsers > 0 ? Math.Round((double)adminCount / totalUsers * 100, 1) : 0
            ),
            new(
                "Teacher",
                teacherCount,
                totalUsers > 0 ? Math.Round((double)teacherCount / totalUsers * 100, 1) : 0
            ),
            new(
                "Student",
                studentCount,
                totalUsers > 0 ? Math.Round((double)studentCount / totalUsers * 100, 1) : 0
            ),
        };

        // Verification status
        var verifiedCount = users.Count(u => u.IsEmailVerified);
        var unverifiedCount = totalUsers - verifiedCount;

        var verificationStatus = new List<VerificationStatusItem>
        {
            new(
                "Verified",
                verifiedCount,
                totalUsers > 0 ? Math.Round((double)verifiedCount / totalUsers * 100, 1) : 0
            ),
            new(
                "Unverified",
                unverifiedCount,
                totalUsers > 0 ? Math.Round((double)unverifiedCount / totalUsers * 100, 1) : 0
            ),
        };

        _logger.LogInformation(
            "Retrieved user dashboard stats: {TotalUsers} users, {Days} days of growth data",
            totalUsers,
            growthData.Count
        );

        return new UserDashboardStatsResponse(growthData, roleDistribution, verificationStatus);
    }
}
