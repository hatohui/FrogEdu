using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DeleteQuestion;

public sealed class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
