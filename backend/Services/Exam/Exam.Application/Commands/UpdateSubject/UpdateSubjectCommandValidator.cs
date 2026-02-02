using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.UpdateSubject;

public sealed class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject ID is required");

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
    }
}
