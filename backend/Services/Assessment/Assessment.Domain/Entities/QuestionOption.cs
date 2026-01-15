using FrogEdu.Assessment.Domain.Enums;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// Question Option entity for multiple choice questions
/// </summary>
public class QuestionOption : Entity
{
    public string OptionText { get; private set; }
    public bool IsCorrect { get; private set; }
    public int OrderIndex { get; private set; }
    public Guid QuestionId { get; private set; }

    // Navigation property
    public Question Question { get; private set; } = null!;

    private QuestionOption() { } // For EF Core

    private QuestionOption(string optionText, bool isCorrect, int orderIndex, Guid questionId)
    {
        if (string.IsNullOrWhiteSpace(optionText))
            throw new ValidationException(nameof(OptionText), "Option text cannot be empty");

        if (orderIndex < 0)
            throw new ValidationException(nameof(OrderIndex), "Order index cannot be negative");

        OptionText = optionText;
        IsCorrect = isCorrect;
        OrderIndex = orderIndex;
        QuestionId = questionId;
    }

    public static QuestionOption Create(
        string optionText,
        bool isCorrect,
        int orderIndex,
        Guid questionId
    )
    {
        return new QuestionOption(optionText, isCorrect, orderIndex, questionId);
    }

    public void UpdateOption(string optionText, bool isCorrect, int orderIndex)
    {
        if (string.IsNullOrWhiteSpace(optionText))
            throw new ValidationException(nameof(OptionText), "Option text cannot be empty");

        OptionText = optionText;
        IsCorrect = isCorrect;
        OrderIndex = orderIndex;
        UpdateTimestamp();
    }
}
