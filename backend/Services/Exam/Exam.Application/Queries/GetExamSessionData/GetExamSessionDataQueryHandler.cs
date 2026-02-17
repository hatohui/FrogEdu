using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamSessionData;

public sealed class GetExamSessionDataQueryHandler(
    IExamRepository examRepository,
    IQuestionRepository questionRepository
) : IRequestHandler<GetExamSessionDataQuery, ExamSessionDataDto?>
{
    private readonly IExamRepository _examRepository = examRepository;
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<ExamSessionDataDto?> Handle(
        GetExamSessionDataQuery request,
        CancellationToken cancellationToken
    )
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            return null;

        var questions = await _questionRepository.GetExamQuestionsAsync(
            request.ExamId,
            cancellationToken
        );

        var questionDtos = questions
            .Select(q => new SessionQuestionDto(
                Id: q.Id,
                Content: q.Content,
                Points: q.Point,
                QuestionType: q.Type,
                ImageUrl: q.MediaUrl,
                Answers: q.Answers.Select(a => new SessionAnswerDto(
                        Id: a.Id,
                        Content: a.Content,
                        IsCorrect: a.IsCorrect
                    ))
                    .ToList()
            ))
            .ToList();

        var totalPoints = questions.Sum(q => q.Point);

        return new ExamSessionDataDto(
            Id: exam.Id,
            Name: exam.Name,
            Description: exam.Description,
            QuestionCount: questions.Count,
            TotalPoints: totalPoints,
            Questions: questionDtos
        );
    }
}
