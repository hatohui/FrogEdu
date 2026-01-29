using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// Represents a student's answer to a question
/// </summary>
public class Answer : Entity
{
    public Guid SubmissionId { get; private set; }
    public Guid QuestionId { get; private set; }
    public Guid[]? SelectedOptionIds { get; private set; }
    public string? AnswerText { get; private set; }
    public decimal? Score { get; private set; }
    public string? Feedback { get; private set; }

    private Answer() { } // EF Core

    public Answer(Guid submissionId, Guid questionId, Guid[]? selectedOptionIds, string? answerText)
    {
        SubmissionId = submissionId;
        QuestionId = questionId;
        SelectedOptionIds = selectedOptionIds;
        AnswerText = answerText;
    }

    public void Grade(decimal score, string? feedback = null)
    {
        Score = score;
        Feedback = feedback;
        MarkAsUpdated();
    }
}
