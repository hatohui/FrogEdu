namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateSubjectRequest(
    string SubjectCode,
    string Name,
    string Description,
    int Grade,
    string? ImageUrl
);
