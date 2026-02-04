using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.AddQuestionsToExam;

public sealed class AddQuestionsToExamCommandHandler : IRequestHandler<AddQuestionsToExamCommand>
{
    private readonly IExamRepository _examRepository;
    private readonly IQuestionRepository _questionRepository;

    public AddQuestionsToExamCommandHandler(
        IExamRepository examRepository,
        IQuestionRepository questionRepository
    )
    {
        _examRepository = examRepository;
        _questionRepository = questionRepository;
    }

    public async Task Handle(AddQuestionsToExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);

        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        // Verify user has permission (must be creator)
        var userId = Guid.Parse(request.UserId);
        if (exam.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to modify this exam");
        }

        // Verify exam is still a draft
        if (!exam.IsDraft)
        {
            throw new InvalidOperationException(
                "Cannot add questions to a published exam. Please archive and create a new version."
            );
        }

        // Validate all questions exist
        foreach (var questionId in request.QuestionIds)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
            if (question is null)
                throw new InvalidOperationException($"Question with ID {questionId} not found");

            // Add question to exam (domain entity will handle duplicates)
            try
            {
                exam.AddQuestion(questionId);
            }
            catch (InvalidOperationException)
            {
                // Question already exists in exam, skip it
                continue;
            }
        }

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);
    }
}
