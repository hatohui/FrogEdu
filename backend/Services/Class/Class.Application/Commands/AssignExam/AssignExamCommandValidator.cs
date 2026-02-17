using FluentValidation;

namespace FrogEdu.Class.Application.Commands.AssignExam;

public sealed class AssignExamCommandValidator : AbstractValidator<AssignExamCommand>
{
    public AssignExamCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty().WithMessage("Class ID is required");

        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.DueDate)
            .WithMessage("Start date must be before due date");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future");

        RuleFor(x => x.Weight)
            .InclusiveBetween(0, 100)
            .WithMessage("Weight must be between 0 and 100");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
