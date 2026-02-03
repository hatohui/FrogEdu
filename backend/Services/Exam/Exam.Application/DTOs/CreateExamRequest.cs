namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateExamRequest(string Name, string Description, Guid SubjectId, int Grade);
