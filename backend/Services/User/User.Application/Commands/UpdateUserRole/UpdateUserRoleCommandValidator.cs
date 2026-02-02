using FluentValidation;

namespace FrogEdu.User.Application.Commands.UpdateUserRole;

public sealed class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role ID is required");
    }
}
