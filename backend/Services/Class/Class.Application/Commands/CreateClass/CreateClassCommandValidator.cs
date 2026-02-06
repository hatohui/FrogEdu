using FluentValidation;

namespace FrogEdu.Class.Application.Commands.CreateClass;

public sealed class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Class name is required")
            .MaximumLength(200)
            .WithMessage("Class name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Grade)
            .NotEmpty()
            .WithMessage("Grade is required")
            .MaximumLength(50)
            .WithMessage("Grade must not exceed 50 characters");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0)
            .WithMessage("Maximum students must be greater than 0")
            .LessThanOrEqualTo(500)
            .WithMessage("Maximum students cannot exceed 500");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.BannerUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.BannerUrl))
            .WithMessage("Banner URL must be a valid URL");
    }
}
