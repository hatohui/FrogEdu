using FrogEdu.Class.Domain.Enums;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// Question aggregate root representing an assessment question
/// </summary>
public class Question : Entity
{
    public string Content { get; private set; }
    public QuestionType Type { get; private set; }
    public Difficulty Difficulty { get; private set; }
    public decimal Points { get; private set; }
    public Guid TextbookId { get; private set; }
    public Guid? ChapterId { get; private set; }
    public string? Explanation { get; private set; }
    public string? ImageS3Key { get; private set; }
    public string? LearningObjectives { get; private set; }

    // Navigation properties
    private readonly List<QuestionOption> _options = new();
    public IReadOnlyCollection<QuestionOption> Options => _options.AsReadOnly();

    private Question() { } // For EF Core

    private Question(
        string content,
        QuestionType type,
        Difficulty difficulty,
        decimal points,
        Guid textbookId,
        Guid? chapterId,
        string? explanation,
        string? imageS3Key,
        string? learningObjectives
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException(nameof(Content), "Question content cannot be empty");

        if (points <= 0)
            throw new ValidationException(nameof(Points), "Points must be greater than 0");

        if (!Enum.IsDefined(typeof(QuestionType), type))
            throw new ValidationException(nameof(Type), "Invalid question type");

        if (!Enum.IsDefined(typeof(Difficulty), difficulty))
            throw new ValidationException(nameof(Difficulty), "Invalid difficulty level");

        Content = content;
        Type = type;
        Difficulty = difficulty;
        Points = points;
        TextbookId = textbookId;
        ChapterId = chapterId;
        Explanation = explanation;
        ImageS3Key = imageS3Key;
        LearningObjectives = learningObjectives;
    }

    public static Question Create(
        string content,
        QuestionType type,
        Difficulty difficulty,
        decimal points,
        Guid textbookId,
        Guid? chapterId = null,
        string? explanation = null,
        string? imageS3Key = null,
        string? learningObjectives = null
    )
    {
        return new Question(
            content,
            type,
            difficulty,
            points,
            textbookId,
            chapterId,
            explanation,
            imageS3Key,
            learningObjectives
        );
    }

    public void UpdateContent(string content, string? explanation, string? learningObjectives)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException(nameof(Content), "Question content cannot be empty");

        Content = content;
        Explanation = explanation;
        LearningObjectives = learningObjectives;
        UpdateTimestamp();
    }

    public void UpdateDifficulty(Difficulty difficulty)
    {
        if (!Enum.IsDefined(typeof(Difficulty), difficulty))
            throw new ValidationException(nameof(Difficulty), "Invalid difficulty level");

        Difficulty = difficulty;
        UpdateTimestamp();
    }

    public void UpdateImage(string imageS3Key)
    {
        ImageS3Key = imageS3Key;
        UpdateTimestamp();
    }

    public QuestionOption AddOption(string optionText, bool isCorrect, int orderIndex)
    {
        if (Type != QuestionType.MCQ && Type != QuestionType.TrueFalse)
            throw new BusinessRuleViolationException(
                "Options can only be added to MCQ or TrueFalse questions"
            );

        // Ensure only one correct answer for MCQ
        if (isCorrect && Type == QuestionType.MCQ && _options.Any(o => o.IsCorrect))
            throw new BusinessRuleViolationException(
                "MCQ questions can only have one correct answer"
            );

        var option = QuestionOption.Create(optionText, isCorrect, orderIndex, Id);
        _options.Add(option);
        UpdateTimestamp();
        return option;
    }

    public void RemoveOption(Guid optionId)
    {
        var option = _options.FirstOrDefault(o => o.Id == optionId);
        if (option == null)
            throw new NotFoundException(nameof(QuestionOption), optionId);

        _options.Remove(option);
        UpdateTimestamp();
    }

    public void ValidateOptions()
    {
        if (Type == QuestionType.MCQ || Type == QuestionType.TrueFalse)
        {
            if (!_options.Any())
                throw new BusinessRuleViolationException($"{Type} questions must have options");

            if (!_options.Any(o => o.IsCorrect))
                throw new BusinessRuleViolationException(
                    "At least one option must be marked as correct"
                );

            if (Type == QuestionType.TrueFalse && _options.Count != 2)
                throw new BusinessRuleViolationException(
                    "True/False questions must have exactly 2 options"
                );
        }
    }
}
