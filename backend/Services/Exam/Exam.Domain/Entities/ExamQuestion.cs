namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Join entity for many-to-many relationship between Exam and Question
/// </summary>
public sealed class ExamQuestion
{
    public Guid ExamId { get; private set; }
    public Guid QuestionId { get; private set; }

    private ExamQuestion() { }

    private ExamQuestion(Guid examId, Guid questionId)
    {
        ExamId = examId;
        QuestionId = questionId;
    }

    public static ExamQuestion Create(Guid examId, Guid questionId)
    {
        if (examId == Guid.Empty)
            throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
        if (questionId == Guid.Empty)
            throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

        return new ExamQuestion(examId, questionId);
    }
}
