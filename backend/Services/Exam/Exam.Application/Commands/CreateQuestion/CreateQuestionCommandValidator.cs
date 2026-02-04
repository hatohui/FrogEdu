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

        RuleFor(x => x.Answers).NotEmpty().WithMessage("At least one answer is required");

        // Minimum answer count depends on question type
        RuleFor(x => x)
            .Must(x =>
                x.Type == Domain.Enums.QuestionType.Essay
                || x.Type == Domain.Enums.QuestionType.FillInTheBlank
                || x.Answers.Count >= 2
            )
            .WithMessage("At least 2 answers are required for this question type");

        RuleFor(x => x.Answers)
            .Must(answers => answers.Any(a => a.IsCorrect))
            .WithMessage("At least one answer must be marked as correct");

        // Only MultipleAnswer type can have multiple correct answers
        RuleFor(x => x)
            .Must(x =>
            {
                var correctCount = x.Answers.Count(a => a.IsCorrect);
                if (correctCount > 1)
                {
                    // Allow FillInTheBlank and MultipleAnswer to have multiple correct
                    return x.Type == Domain.Enums.QuestionType.MultipleAnswer
                        || x.Type == Domain.Enums.QuestionType.FillInTheBlank;
                }
                return true;
            })
            .WithMessage(
                "Only Multiple Answer and Fill in the Blank questions can have more than one correct answer"
            );

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
