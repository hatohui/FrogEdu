using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetTopics;

public sealed class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, GetTopicsResponse>
{
    private readonly ITopicRepository _topicRepository;

    public GetTopicsQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<GetTopicsResponse> Handle(
        GetTopicsQuery request,
        CancellationToken cancellationToken
    )
    {
        var topics = await _topicRepository.GetBySubjectIdAsync(
            request.SubjectId,
            cancellationToken
        );

        var topicDtos = topics
            .Select(t => new TopicDto(t.Id, t.Title, t.Description, t.IsCurriculum, t.SubjectId))
            .ToList();

        return new GetTopicsResponse(topicDtos);
    }
}
