using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteTopic;

public sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Unit>
{
    private readonly ITopicRepository _topicRepository;

    public DeleteTopicCommandHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<Unit> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            throw new InvalidOperationException($"Topic with ID {request.TopicId} not found");

        // Verify the user owns this topic
        if (topic.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to delete this topic");

        _topicRepository.Delete(topic);
        await _topicRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
