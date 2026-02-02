using FluentValidation;

namespace FrogEdu.User.Application.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
