using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteQuestion;

public sealed record DeleteQuestionCommand(Guid QuestionId, string UserId) : IRequest<Unit>;
