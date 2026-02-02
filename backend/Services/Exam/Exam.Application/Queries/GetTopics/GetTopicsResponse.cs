namespace FrogEdu.Exam.Application.Queries.GetTopics;

public sealed record TopicDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCurriculum,
    Guid SubjectId
);

public sealed record GetTopicsResponse(List<TopicDto> Topics);
