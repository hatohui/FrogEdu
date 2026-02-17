using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserStatistics;

public sealed record GetUserStatisticsQuery() : IRequest<UserStatisticsResponse>;

public sealed record UserStatisticsResponse(
    int TotalUsers,
    int TotalAdmins,
    int TotalTeachers,
    int TotalStudents,
    int VerifiedUsers,
    int UnverifiedUsers,
    int UsersCreatedLast30Days,
    int UsersCreatedLast7Days
);
