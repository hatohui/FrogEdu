using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
{
    public CreateExamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");

        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject ID is required");

        RuleFor(x => x.Grade)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Grade must be at least 1")
            .LessThanOrEqualTo(5)
            .WithMessage("Grade cannot exceed 5 (primary school)");
    }
}
