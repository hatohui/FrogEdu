using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentExamSessions;

/// <summary>
/// Get exam sessions for the current user across their enrolled classes.
/// All roles (including Admin) see only sessions for classes they are personally enrolled in.
/// </summary>
public sealed record GetStudentExamSessionsQuery(
    string StudentId,
    string Role,
    bool UpcomingOnly = false
) : IRequest<IReadOnlyList<ExamSessionResponse>>;
