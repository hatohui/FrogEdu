using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DeleteExam;

public sealed class DeleteExamCommandValidator : AbstractValidator<DeleteExamCommand>
{
    public DeleteExamCommandValidator()
    {
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
