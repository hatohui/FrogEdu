using FrogEdu.Exam.Domain.Enums;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateQuestion;

public sealed record UpdateQuestionCommand(
    Guid QuestionId,
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    string? MediaUrl,
    string UserId
) : IRequest<Unit>;
