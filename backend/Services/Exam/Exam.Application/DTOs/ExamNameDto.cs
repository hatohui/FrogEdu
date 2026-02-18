namespace FrogEdu.Exam.Application.DTOs;

/// <summary>
/// Lightweight DTO returned by the internal batch-names endpoint.
/// Contains only the exam ID and name — safe to expose without authentication.
/// </summary>
public sealed record ExamNameDto(Guid Id, string Name);
