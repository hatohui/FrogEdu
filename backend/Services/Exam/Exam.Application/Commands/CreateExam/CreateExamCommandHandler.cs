using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed class CreateExamCommandHandler
    : IRequestHandler<CreateExamCommand, CreateExamResponse>
{
    private readonly IExamRepository _examRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ISubjectRepository _subjectRepository;

    public CreateExamCommandHandler(
        IExamRepository examRepository,
        ITopicRepository topicRepository,
        ISubjectRepository subjectRepository
    )
    {
        _examRepository = examRepository;
        _topicRepository = topicRepository;
        _subjectRepository = subjectRepository;
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

        // Verify subject exists
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        var exam = Domain.Entities.Exam.Create(
            request.Name,
            request.Description,
            request.TopicId,
            request.SubjectId,
            request.Grade,
            request.UserId
        );

        await _examRepository.AddAsync(exam, cancellationToken);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return new CreateExamResponse(
            exam.Id,
            exam.Name,
            exam.Description,
            exam.TopicId,
            exam.SubjectId,
            exam.Grade,
            exam.IsDraft,
            exam.IsActive,
            exam.CreatedAt
        );
    }
}
