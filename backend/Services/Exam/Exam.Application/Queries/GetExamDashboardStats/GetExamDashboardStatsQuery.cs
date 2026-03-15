using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamDashboardStats;

public sealed record GetExamDashboardStatsQuery() : IRequest<ExamDashboardStatsResponse>;

public sealed record ExamDashboardStatsResponse(
    int TotalExams,
    int ActiveExams,
    int DraftExams,
    int TotalQuestions,
    int PublicQuestions
);
