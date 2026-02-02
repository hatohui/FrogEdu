using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetTopicById;

public sealed class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, TopicDto?>
{
    private readonly ITopicRepository _topicRepository;

    public GetTopicByIdQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<TopicDto?> Handle(
        GetTopicByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);

        if (topic is null)
            return null;

        return new TopicDto(
            topic.Id,
            topic.Title,
            topic.Description,
            topic.IsCurriculum,
            topic.SubjectId
        );
    }
}
