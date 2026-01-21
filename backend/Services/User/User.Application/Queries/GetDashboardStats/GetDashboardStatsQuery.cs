using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetDashboardStats;

/// <summary>
/// Query to get dashboard statistics for a user
/// </summary>
public sealed record GetDashboardStatsQuery(Guid UserId, bool IsTeacher)
    : IRequest<DashboardStatsDto>;
