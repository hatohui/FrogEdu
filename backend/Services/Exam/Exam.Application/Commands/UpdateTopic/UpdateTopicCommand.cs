using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateTopic;

public sealed record UpdateTopicCommand(
    Guid TopicId,
    string Title,
    string Description,
    string UserId
) : IRequest<Unit>;
