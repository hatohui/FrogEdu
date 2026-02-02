using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateTopic;

public sealed class CreateTopicCommandHandler
    : IRequestHandler<CreateTopicCommand, CreateTopicResponse>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ISubjectRepository _subjectRepository;

    public CreateTopicCommandHandler(
        ITopicRepository topicRepository,
        ISubjectRepository subjectRepository
    )
    {
        _topicRepository = topicRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<CreateTopicResponse> Handle(
        CreateTopicCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify subject exists
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        var topic = Domain.Entities.Topic.Create(
            request.Title,
            request.Description,
            request.IsCurriculum,
            request.SubjectId,
            request.UserId
        );

        await _topicRepository.AddAsync(topic, cancellationToken);
        await _topicRepository.SaveChangesAsync(cancellationToken);

        return new CreateTopicResponse(
            topic.Id,
            topic.Title,
            topic.Description,
            topic.IsCurriculum,
            topic.SubjectId,
            topic.CreatedAt
        );
    }
}
