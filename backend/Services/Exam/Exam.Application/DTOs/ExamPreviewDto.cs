namespace FrogEdu.Exam.Application.DTOs;

public sealed record ExamPreviewDto(
    Guid Id,
    string Name,
    string Description,
    string SubjectName,
    int Grade,
    int QuestionCount,
    double TotalPoints,
    DateTime CreatedAt,
    IReadOnlyList<ExamPreviewQuestionDto> Questions
);

public sealed record ExamPreviewQuestionDto(
    int QuestionNumber,
    string Content,
    double Point,
    string Type,
    string CognitiveLevel,
    string? MediaUrl,
    IReadOnlyList<ExamPreviewAnswerDto> Answers
);

public sealed record ExamPreviewAnswerDto(string Label, string Content, bool IsCorrect);
