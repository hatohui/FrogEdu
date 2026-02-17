namespace FrogEdu.Class.Application.Interfaces;

public interface IExamServiceClient
{
    /// <summary>
    /// Fetch exam questions with correct answers for grading purposes.
    /// </summary>
    Task<ExamWithQuestionsDto?> GetExamWithAnswersAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    );
}

public sealed record ExamWithQuestionsDto(
    Guid Id,
    string Name,
    int QuestionCount,
    List<ExamQuestionDto> Questions
);

public sealed record ExamQuestionDto(
    Guid Id,
    string Content,
    double Point,
    string Type,
    List<ExamAnswerDto> Answers
);

public sealed record ExamAnswerDto(Guid Id, string Content, bool IsCorrect);
