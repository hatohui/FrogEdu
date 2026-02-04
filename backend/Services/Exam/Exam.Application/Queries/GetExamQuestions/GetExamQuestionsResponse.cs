using FrogEdu.Exam.Application.DTOs;

namespace FrogEdu.Exam.Application.Queries.GetExamQuestions;

public sealed record GetExamQuestionsResponse(List<QuestionDto> Questions);
