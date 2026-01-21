using FluentValidation;

namespace FrogEdu.User.Application.Commands.CreateClass;

/// <summary>
/// Validator for CreateClassCommand
/// </summary>
public sealed class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Class name is required")
            .MaximumLength(100)
            .WithMessage("Class name cannot exceed 100 characters");

        RuleFor(x => x.Grade)
            .InclusiveBetween((short)1, (short)12)
            .WithMessage("Grade must be between 1 and 12");

        RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Teacher ID is required");

        RuleFor(x => x.Subject)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Subject))
            .WithMessage("Subject cannot exceed 50 characters");

        RuleFor(x => x.School)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.School))
            .WithMessage("School name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0)
            .When(x => x.MaxStudents.HasValue)
            .WithMessage("Maximum students must be greater than 0");
    }
}
