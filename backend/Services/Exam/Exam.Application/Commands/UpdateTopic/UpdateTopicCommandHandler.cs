using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateTopic;

public sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Unit>
{
    private readonly ITopicRepository _topicRepository;

    public UpdateTopicCommandHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<Unit> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            throw new InvalidOperationException($"Topic with ID {request.TopicId} not found");

        // Verify the user owns this topic
        if (topic.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to update this topic");

        topic.Update(request.Title, request.Description, request.UserId);

        _topicRepository.Update(topic);
        await _topicRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
