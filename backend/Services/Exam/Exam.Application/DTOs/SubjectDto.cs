namespace FrogEdu.Exam.Application.DTOs;

public sealed record SubjectDto(
    Guid Id,
    string SubjectCode,
    string Name,
    string Description,
    string ImageUrl,
    int Grade
);
