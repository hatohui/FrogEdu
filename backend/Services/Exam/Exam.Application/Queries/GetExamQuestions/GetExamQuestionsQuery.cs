using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamQuestions;

public sealed record GetExamQuestionsQuery(Guid ExamId, string UserId)
    : IRequest<GetExamQuestionsResponse>;
