using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetSessionAttempts;

/// <summary>
/// Get all attempts for an exam session (Teacher view)
/// </summary>
public sealed record GetSessionAttemptsQuery(Guid ExamSessionId)
    : IRequest<IReadOnlyList<StudentExamAttemptResponse>>;
