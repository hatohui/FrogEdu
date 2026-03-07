using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.RecordAIUsage;

public sealed class RecordAIUsageCommandValidator : AbstractValidator<RecordAIUsageCommand>
{
    public RecordAIUsageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.ActionType)
            .NotEmpty()
            .WithMessage("Action type is required")
            .MaximumLength(100)
            .WithMessage("Action type must be 100 characters or less");
    }
}
