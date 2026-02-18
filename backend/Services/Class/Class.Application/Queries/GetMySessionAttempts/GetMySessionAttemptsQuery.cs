using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetMySessionAttempts;

/// <summary>
/// Get all attempts the current student has made for a given exam session.
/// Returns scores so the student can review their own performance.
/// </summary>
public sealed record GetMySessionAttemptsQuery(Guid ExamSessionId, string StudentId)
    : IRequest<IReadOnlyList<StudentExamAttemptResponse>>;
