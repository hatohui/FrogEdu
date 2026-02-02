namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateTopicRequest(string Title, string Description, bool IsCurriculum);
