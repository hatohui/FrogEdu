using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DeleteTopic;

public sealed class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
{
    public DeleteTopicCommandValidator()
    {
        RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
