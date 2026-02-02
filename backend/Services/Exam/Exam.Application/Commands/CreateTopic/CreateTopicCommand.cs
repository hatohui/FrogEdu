using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateTopic;

public sealed record CreateTopicCommand(
    string Title,
    string Description,
    bool IsCurriculum,
    Guid SubjectId,
    string UserId
) : IRequest<CreateTopicResponse>;

public sealed record CreateTopicResponse(
    Guid Id,
    string Title,
    string Description,
    bool IsCurriculum,
    Guid SubjectId,
    DateTime CreatedAt
);
