using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetUserStatistics;

public sealed class GetUserStatisticsQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserStatisticsQueryHandler> logger
) : IRequestHandler<GetUserStatisticsQuery, UserStatisticsResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<GetUserStatisticsQueryHandler> _logger = logger;

    public async Task<UserStatisticsResponse> Handle(
        GetUserStatisticsQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var last30Days = now.AddDays(-30);
        var last7Days = now.AddDays(-7);

        var totalUsers = users.Count;
        var totalAdmins = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Admin");
        var totalTeachers = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Teacher");
        var totalStudents = users.Count(u => RoleConstants.MapRoleIdToName(u.RoleId) == "Student");
        var verifiedUsers = users.Count(u => u.IsEmailVerified);
        var unverifiedUsers = totalUsers - verifiedUsers;
        var usersCreatedLast30Days = users.Count(u => u.CreatedAt >= last30Days);
        var usersCreatedLast7Days = users.Count(u => u.CreatedAt >= last7Days);

        _logger.LogInformation("Retrieved user statistics: {TotalUsers} total users", totalUsers);

        return new UserStatisticsResponse(
            totalUsers,
            totalAdmins,
            totalTeachers,
            totalStudents,
            verifiedUsers,
            unverifiedUsers,
            usersCreatedLast30Days,
            usersCreatedLast7Days
        );
    }
}
