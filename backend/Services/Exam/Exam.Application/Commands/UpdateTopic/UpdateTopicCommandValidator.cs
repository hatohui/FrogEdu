using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.UpdateTopic;

public sealed class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
{
    public UpdateTopicCommandValidator()
    {
        RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Topic title is required")
            .MaximumLength(256)
            .WithMessage("Topic title must not exceed 256 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Topic description must not exceed 1000 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
