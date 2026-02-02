using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.DeleteSubject;

public sealed class DeleteSubjectCommandValidator : AbstractValidator<DeleteSubjectCommand>
{
    public DeleteSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject ID is required");
    }
}
