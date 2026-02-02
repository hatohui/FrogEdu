using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.UpdateMatrix;

public sealed class UpdateMatrixCommandValidator : AbstractValidator<UpdateMatrixCommand>
{
    public UpdateMatrixCommandValidator()
    {
        RuleFor(x => x.MatrixId).NotEmpty().WithMessage("Matrix ID is required");

        RuleFor(x => x.MatrixTopics).NotEmpty().WithMessage("Matrix topics are required");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
