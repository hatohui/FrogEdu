using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DetachMatrixFromExam;

public sealed class DetachMatrixFromExamCommandValidator
    : AbstractValidator<DetachMatrixFromExamCommand>
{
    public DetachMatrixFromExamCommandValidator()
    {
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
