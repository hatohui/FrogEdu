using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentExamSessions;

/// <summary>
/// Get all active/upcoming exam sessions for a student across enrolled classes
/// </summary>
public sealed record GetStudentExamSessionsQuery(string StudentId, bool UpcomingOnly = false)
    : IRequest<IReadOnlyList<ExamSessionResponse>>;
