using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamDashboardStats;

public sealed class GetExamDashboardStatsQueryHandler(
    IExamRepository examRepository,
    IQuestionRepository questionRepository
) : IRequestHandler<GetExamDashboardStatsQuery, ExamDashboardStatsResponse>
{
    private readonly IExamRepository _examRepository = examRepository;
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<ExamDashboardStatsResponse> Handle(
        GetExamDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        var allExams = await _examRepository.GetAllExamsAsync(cancellationToken);
        var publicQuestions = await _questionRepository.GetPublicQuestionsAsync(cancellationToken);
        var activeExams = allExams.Where(e => e.IsActive && !e.IsDraft).ToList();
        var draftExams = allExams.Where(e => e.IsDraft).ToList();

        // Count total questions via all exam questions aggregated
        var allQuestions = activeExams
            .Concat(draftExams)
            .SelectMany(e => e.ExamQuestions)
            .Select(eq => eq.QuestionId)
            .Distinct()
            .Count();

        return new ExamDashboardStatsResponse(
            TotalExams: allExams.Count,
            ActiveExams: activeExams.Count,
            DraftExams: draftExams.Count,
            TotalQuestions: allQuestions,
            PublicQuestions: publicQuestions.Count
        );
    }
}
