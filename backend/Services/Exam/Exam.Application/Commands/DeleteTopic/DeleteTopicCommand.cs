using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteTopic;

public sealed record DeleteTopicCommand(Guid TopicId, string UserId) : IRequest<Unit>;
