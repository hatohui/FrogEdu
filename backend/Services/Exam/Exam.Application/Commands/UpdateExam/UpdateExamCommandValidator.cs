using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.UpdateExam;

public sealed class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Exam name is required")
            .MaximumLength(256)
            .WithMessage("Exam name must not exceed 256 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Exam description is required")
            .MaximumLength(1000)
            .WithMessage("Exam description must not exceed 1000 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
