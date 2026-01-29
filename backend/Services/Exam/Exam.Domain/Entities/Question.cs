using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Question entity - represents a question in the question bank
/// </summary>
public sealed class Question : Entity
{
    public string Content { get; private set; } = null!;
    public QuestionType Type { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public Guid? ChapterId { get; private set; }
    public string? CorrectAnswer { get; private set; }

    private Question() { }

    private Question(
        string content,
        QuestionType type,
        DifficultyLevel difficulty,
        Guid? chapterId = null
    )
    {
        Content = content;
        Type = type;
        Difficulty = difficulty;
        ChapterId = chapterId;
    }

    public static Question Create(
        string content,
        QuestionType type,
        DifficultyLevel difficulty,
        Guid? chapterId = null
    )
    {
        return new Question(content, type, difficulty, chapterId);
    }

    public void UpdateContent(string content)
    {
        Content = content;
        UpdateTimestamp();
    }

    public void SetCorrectAnswer(string answer)
    {
        CorrectAnswer = answer;
        UpdateTimestamp();
    }
}

public enum QuestionType
{
    MultipleChoice = 1,
    Essay = 2,
    TrueFalse = 3,
    ShortAnswer = 4,
}

public enum DifficultyLevel
{
    Easy = 1,
    Medium = 2,
    Hard = 3,
}
