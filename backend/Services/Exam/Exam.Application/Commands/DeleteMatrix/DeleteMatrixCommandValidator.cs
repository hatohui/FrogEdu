using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DeleteMatrix;

public sealed class DeleteMatrixCommandValidator : AbstractValidator<DeleteMatrixCommand>
{
    public DeleteMatrixCommandValidator()
    {
        RuleFor(x => x.MatrixId).NotEmpty().WithMessage("Matrix ID is required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
