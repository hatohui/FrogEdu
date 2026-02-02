using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.UpdateQuestion;

public sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question ID is required");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Question content is required")
            .MaximumLength(2000)
            .WithMessage("Question content must not exceed 2000 characters");

        RuleFor(x => x.Point).GreaterThan(0).WithMessage("Point must be greater than 0");

        RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid question type");

        RuleFor(x => x.CognitiveLevel).IsInEnum().WithMessage("Invalid cognitive level");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
