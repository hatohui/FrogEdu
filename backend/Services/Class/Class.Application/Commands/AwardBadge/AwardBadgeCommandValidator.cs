using FluentValidation;

namespace FrogEdu.Class.Application.Commands.AwardBadge;

public sealed class AwardBadgeCommandValidator : AbstractValidator<AwardBadgeCommand>
{
    public AwardBadgeCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student ID is required");
        RuleFor(x => x.BadgeId).NotEmpty().WithMessage("Badge ID is required");
        RuleFor(x => x.ClassId).NotEmpty().WithMessage("Class ID is required");
        RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Teacher ID is required");
        RuleFor(x => x.CustomPraise)
            .MaximumLength(500)
            .WithMessage("Custom praise must be 500 characters or less");
    }
}
