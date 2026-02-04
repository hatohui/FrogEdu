using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.CreateSubscriptionTier;

public sealed class CreateSubscriptionTierCommandValidator
    : AbstractValidator<CreateSubscriptionTierCommand>
{
    public CreateSubscriptionTierCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required")
            .Length(3)
            .WithMessage("Currency must be a 3-letter ISO code");

        RuleFor(x => x.DurationInDays)
            .GreaterThan(0)
            .WithMessage("Duration must be greater than 0 days");

        RuleFor(x => x.TargetRole)
            .NotEmpty()
            .WithMessage("Target role is required")
            .Must(role => role == "Student" || role == "Teacher" || role == "Admin")
            .WithMessage("Target role must be Student, Teacher, or Admin");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .WithMessage("Image URL cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
