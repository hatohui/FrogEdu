using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamPreview;

public sealed class GetExamPreviewQueryHandler(
    IExamRepository examRepository,
    IQuestionRepository questionRepository,
    ISubjectRepository subjectRepository
) : IRequestHandler<GetExamPreviewQuery, ExamPreviewDto?>
{
    private readonly IExamRepository _examRepository = examRepository;
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly ISubjectRepository _subjectRepository = subjectRepository;

    public async Task<ExamPreviewDto?> Handle(
        GetExamPreviewQuery request,
        CancellationToken cancellationToken
    )
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);

        // Check if exam exists and user has access (convert string userId to Guid for comparison)
        if (exam is null)
            return null;

        if (!Guid.TryParse(request.UserId, out var userIdGuid) || exam.CreatedBy != userIdGuid)
            return null;

        var subject = await _subjectRepository.GetByIdAsync(exam.SubjectId, cancellationToken);
        if (subject is null)
            return null;

        var questions = await _questionRepository.GetExamQuestionsAsync(
            request.ExamId,
            cancellationToken
        );

        var questionDtos = questions
            .Select(
                (q, index) =>
                    new ExamPreviewQuestionDto(
                        QuestionNumber: index + 1,
                        Content: q.Content,
                        Point: q.Point,
                        Type: GetQuestionTypeLabel(q.Type),
                        CognitiveLevel: GetCognitiveLevelLabel(q.CognitiveLevel),
                        MediaUrl: q.MediaUrl,
                        Answers: q.Answers.Select(
                                (a, answerIndex) =>
                                    new ExamPreviewAnswerDto(
                                        Label: GetAnswerLabel(answerIndex),
                                        Content: a.Content,
                                        IsCorrect: a.IsCorrect
                                    )
                            )
                            .ToList()
                    )
            )
            .ToList();

        var totalPoints = questions.Sum(q => q.Point);

        return new ExamPreviewDto(
            Id: exam.Id,
            Name: exam.Name,
            Description: exam.Description,
            SubjectName: subject.Name,
            Grade: exam.Grade,
            QuestionCount: questions.Count,
            TotalPoints: totalPoints,
            CreatedAt: exam.CreatedAt,
            Questions: questionDtos
        );
    }

    private static string GetQuestionTypeLabel(QuestionType type) =>
        type switch
        {
            QuestionType.MultipleChoice => "Multiple Choice",
            QuestionType.MultipleAnswer => "Multiple Answer",
            QuestionType.TrueFalse => "True/False",
            QuestionType.FillInTheBlank => "Fill in the Blank",
            QuestionType.Essay => "Essay",
            _ => type.ToString(),
        };

    private static string GetCognitiveLevelLabel(CognitiveLevel level) =>
        level switch
        {
            CognitiveLevel.Remember => "Remember",
            CognitiveLevel.Understand => "Understand",
            CognitiveLevel.Apply => "Apply",
            CognitiveLevel.Analyze => "Analyze",
            _ => level.ToString(),
        };

    private static string GetAnswerLabel(int index)
    {
        const string labels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return index < labels.Length ? labels[index].ToString() : $"Option {index + 1}";
    }
}
