using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamQuestions;

public sealed class GetExamQuestionsQueryHandler
    : IRequestHandler<GetExamQuestionsQuery, GetExamQuestionsResponse>
{
    private readonly IExamRepository _examRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetExamQuestionsQueryHandler(
        IExamRepository examRepository,
        IQuestionRepository questionRepository
    )
    {
        _examRepository = examRepository;
        _questionRepository = questionRepository;
    }

    public async Task<GetExamQuestionsResponse> Handle(
        GetExamQuestionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);

        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        // Verify user has access (either creator or if exam is active/published)
        var userId = Guid.Parse(request.UserId);
        if (exam.CreatedBy != userId && !exam.IsActive)
        {
            throw new UnauthorizedAccessException(
                "You do not have permission to view this exam's questions"
            );
        }

        // Get all questions for this exam
        var questionIds = exam.ExamQuestions.Select(eq => eq.QuestionId).ToList();
        var questions = new List<QuestionDto>();

        foreach (var questionId in questionIds)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
            if (question is not null)
            {
                questions.Add(
                    new QuestionDto(
                        question.Id,
                        question.Content,
                        question.Point,
                        question.Type,
                        question.CognitiveLevel,
                        question.Source,
                        question.TopicId,
                        question.MediaUrl,
                        question.IsPublic,
                        question.Answers.Count,
                        question.CreatedAt
                    )
                );
            }
        }

        return new GetExamQuestionsResponse(questions);
    }
}
