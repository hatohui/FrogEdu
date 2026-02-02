using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
{
    public CreateExamCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(0)
            .WithMessage("Duration must be greater than 0 minutes")
            .LessThanOrEqualTo(480)
            .WithMessage("Duration cannot exceed 480 minutes (8 hours)");

        RuleFor(x => x.PassScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Pass score must be greater than or equal to 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Pass score cannot exceed 100");

        RuleFor(x => x.MaxAttempts)
            .GreaterThan(0)
            .WithMessage("Max attempts must be greater than 0")
            .LessThanOrEqualTo(10)
            .WithMessage("Max attempts cannot exceed 10");

        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");
    }
}
