using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Enums;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetQuestions;

public sealed record GetQuestionsQuery(
    Guid? TopicId,
    CognitiveLevel? CognitiveLevel,
    bool? IsPublic,
    string UserId
) : IRequest<GetQuestionsResponse>;
