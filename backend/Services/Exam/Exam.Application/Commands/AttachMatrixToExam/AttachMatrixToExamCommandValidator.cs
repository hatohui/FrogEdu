using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.AttachMatrixToExam;

public sealed class AttachMatrixToExamCommandValidator
    : AbstractValidator<AttachMatrixToExamCommand>
{
    public AttachMatrixToExamCommandValidator()
    {
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

        RuleFor(x => x.MatrixId).NotEmpty().WithMessage("Matrix ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
