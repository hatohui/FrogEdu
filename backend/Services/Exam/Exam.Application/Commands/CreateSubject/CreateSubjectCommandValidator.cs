using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateSubject;

public sealed class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectCode)
            .NotEmpty()
            .WithMessage("Subject code is required")
            .MaximumLength(50)
            .WithMessage("Subject code must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Subject name is required")
            .MaximumLength(256)
            .WithMessage("Subject name must not exceed 256 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Subject description is required")
            .MaximumLength(1000)
            .WithMessage("Subject description must not exceed 1000 characters");

        RuleFor(x => x.Grade)
            .InclusiveBetween(1, 5)
            .WithMessage("Grade must be between 1 and 5 (primary school)");
    }
}
