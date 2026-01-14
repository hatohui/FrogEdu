using FrogEdu.Assessment.Domain.Enums;
using FrogEdu.Assessment.Domain.ValueObjects;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// ExamPaper aggregate root representing a generated exam
/// </summary>
public class ExamPaper : Entity
{
    public string Title { get; private set; }
    public ExamMatrix Matrix { get; private set; }
    public Guid TextbookId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public int DurationMinutes { get; private set; }
    public string? Instructions { get; private set; }
    public string? ExamPdfS3Key { get; private set; }
    public string? AnswerKeyPdfS3Key { get; private set; }
    public int Version { get; private set; }

    // Navigation properties
    private readonly List<ExamQuestion> _examQuestions = new();
    public IReadOnlyCollection<ExamQuestion> ExamQuestions => _examQuestions.AsReadOnly();

    public decimal TotalPoints => _examQuestions.Sum(eq => eq.Points);
    public int TotalQuestions => _examQuestions.Count;

    private ExamPaper() { } // For EF Core

    private ExamPaper(
        string title,
        ExamMatrix matrix,
        Guid textbookId,
        Guid createdByUserId,
        int durationMinutes,
        string? instructions
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Exam title cannot be empty");

        if (durationMinutes <= 0)
            throw new ValidationException(
                nameof(DurationMinutes),
                "Duration must be greater than 0"
            );

        Title = title;
        Matrix = matrix;
        TextbookId = textbookId;
        CreatedByUserId = createdByUserId;
        DurationMinutes = durationMinutes;
        Instructions = instructions;
        Version = 1;
    }

    public static ExamPaper Create(
        string title,
        ExamMatrix matrix,
        Guid textbookId,
        Guid createdByUserId,
        int durationMinutes,
        string? instructions = null
    )
    {
        return new ExamPaper(
            title,
            matrix,
            textbookId,
            createdByUserId,
            durationMinutes,
            instructions
        );
    }

    public void UpdateDetails(string title, int durationMinutes, string? instructions)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Exam title cannot be empty");

        if (durationMinutes <= 0)
            throw new ValidationException(
                nameof(DurationMinutes),
                "Duration must be greater than 0"
            );

        Title = title;
        DurationMinutes = durationMinutes;
        Instructions = instructions;
        UpdateTimestamp();
    }

    public ExamQuestion AddQuestion(Guid questionId, int orderIndex, decimal points)
    {
        // Check for duplicate questions
        if (_examQuestions.Any(eq => eq.QuestionId == questionId))
            throw new BusinessRuleViolationException(
                $"Question {questionId} already exists in this exam"
            );

        // Check if order index already exists
        if (_examQuestions.Any(eq => eq.OrderIndex == orderIndex))
            throw new BusinessRuleViolationException($"Order index {orderIndex} already exists");

        var examQuestion = ExamQuestion.Create(Id, questionId, orderIndex, points);
        _examQuestions.Add(examQuestion);
        UpdateTimestamp();
        return examQuestion;
    }

    public void RemoveQuestion(Guid examQuestionId)
    {
        var examQuestion = _examQuestions.FirstOrDefault(eq => eq.Id == examQuestionId);
        if (examQuestion == null)
            throw new NotFoundException(nameof(ExamQuestion), examQuestionId);

        _examQuestions.Remove(examQuestion);
        UpdateTimestamp();
    }

    public void ReplaceQuestion(Guid oldQuestionId, Guid newQuestionId, decimal newPoints)
    {
        var examQuestion = _examQuestions.FirstOrDefault(eq => eq.QuestionId == oldQuestionId);
        if (examQuestion == null)
            throw new NotFoundException(nameof(ExamQuestion), oldQuestionId);

        var orderIndex = examQuestion.OrderIndex;
        _examQuestions.Remove(examQuestion);

        var newExamQuestion = ExamQuestion.Create(Id, newQuestionId, orderIndex, newPoints);
        _examQuestions.Add(newExamQuestion);

        Version++;
        UpdateTimestamp();
    }

    public void SetPdfKeys(string examPdfS3Key, string answerKeyPdfS3Key)
    {
        if (string.IsNullOrWhiteSpace(examPdfS3Key))
            throw new ValidationException(nameof(ExamPdfS3Key), "Exam PDF S3 key cannot be empty");

        if (string.IsNullOrWhiteSpace(answerKeyPdfS3Key))
            throw new ValidationException(
                nameof(AnswerKeyPdfS3Key),
                "Answer key PDF S3 key cannot be empty"
            );

        ExamPdfS3Key = examPdfS3Key;
        AnswerKeyPdfS3Key = answerKeyPdfS3Key;
        UpdateTimestamp();
    }

    public void ValidateExam()
    {
        if (_examQuestions.Count < 5)
            throw new BusinessRuleViolationException("Exam must have at least 5 questions");

        // Validate that total points match expected
        if (TotalPoints != Matrix.TotalPoints)
            throw new BusinessRuleViolationException(
                $"Total points ({TotalPoints}) do not match matrix expected points ({Matrix.TotalPoints})"
            );

        // Validate question count matches matrix
        if (TotalQuestions != Matrix.TotalQuestions)
            throw new BusinessRuleViolationException(
                $"Total questions ({TotalQuestions}) do not match matrix expected count ({Matrix.TotalQuestions})"
            );
    }
}
