using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassDashboardStats;

public sealed record GetClassDashboardStatsQuery() : IRequest<ClassDashboardStatsResponse>;

public sealed record ClassDashboardStatsResponse(
    int TotalClasses,
    int ActiveClasses,
    int TotalExamSessions,
    int ActiveExamSessions,
    int TotalAttempts,
    int SubmittedAttempts
);
