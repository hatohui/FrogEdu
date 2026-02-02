using FrogEdu.Exam.Domain.Enums;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetQuestions;

public sealed record GetQuestionsQuery(
    Guid? TopicId = null,
    CognitiveLevel? CognitiveLevel = null,
    bool? IsPublic = null
) : IRequest<GetQuestionsResponse>;
