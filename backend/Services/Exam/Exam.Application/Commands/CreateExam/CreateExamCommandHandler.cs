using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed class CreateExamCommandHandler
    : IRequestHandler<CreateExamCommand, CreateExamResponse>
{
    private readonly IExamRepository _examRepository;
    private readonly ITopicRepository _topicRepository;

    public CreateExamCommandHandler(
        IExamRepository examRepository,
        ITopicRepository topicRepository
    )
    {
        _examRepository = examRepository;
        _topicRepository = topicRepository;
    }

    public async Task<CreateExamResponse> Handle(
        CreateExamCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify topic exists
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            throw new InvalidOperationException($"Topic with ID {request.TopicId} not found");

        var exam = Domain.Entities.Exam.Create(
            request.Title,
            request.Duration,
            request.PassScore,
            request.MaxAttempts,
            request.StartTime,
            request.EndTime,
            request.TopicId,
            request.UserId,
            request.ShouldShuffleQuestions,
            request.ShouldShuffleAnswerOptions
        );

        await _examRepository.AddAsync(exam, cancellationToken);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return new CreateExamResponse(
            exam.Id,
            exam.Title,
            exam.Duration,
            exam.PassScore,
            exam.MaxAttempts,
            exam.StartTime,
            exam.EndTime,
            exam.TopicId,
            exam.IsDraft,
            exam.IsActive,
            exam.CreatedAt
        );
    }
}
