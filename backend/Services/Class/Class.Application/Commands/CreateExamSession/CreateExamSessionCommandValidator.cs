using FluentValidation;

namespace FrogEdu.Class.Application.Commands.CreateExamSession;

public sealed class CreateExamSessionCommandValidator : AbstractValidator<CreateExamSessionCommand>
{
    public CreateExamSessionCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty().WithMessage("Class ID is required");
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required");
        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");
        RuleFor(x => x.RetryTimes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Retry times cannot be negative");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
