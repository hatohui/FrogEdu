using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateQuestion;

public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required")
            .MaximumLength(2000)
            .WithMessage("Content cannot exceed 2000 characters");

        RuleFor(x => x.Point)
            .GreaterThan(0)
            .WithMessage("Point must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Point cannot exceed 100");

        RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");

        RuleFor(x => x.Answers)
            .NotEmpty()
            .WithMessage("At least one answer is required")
            .Must(answers => answers.Count >= 2)
            .WithMessage("At least 2 answers are required");

        RuleFor(x => x.Answers)
            .Must(answers => answers.Any(a => a.IsCorrect))
            .WithMessage("At least one answer must be marked as correct");

        RuleForEach(x => x.Answers)
            .ChildRules(answer =>
            {
                answer
                    .RuleFor(x => x.Content)
                    .NotEmpty()
                    .WithMessage("Answer content is required")
                    .MaximumLength(500)
                    .WithMessage("Answer content cannot exceed 500 characters");
            });
    }
}
