using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentExamSessions;

/// <summary>
/// Get exam sessions for the current user across their enrolled (or all, for Admin) classes
/// </summary>
public sealed record GetStudentExamSessionsQuery(
    string StudentId,
    string Role,
    bool UpcomingOnly = false
) : IRequest<IReadOnlyList<ExamSessionResponse>>;
