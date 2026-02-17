using FluentValidation;

namespace FrogEdu.Class.Application.Commands.JoinClass;

public sealed class JoinClassCommandValidator : AbstractValidator<JoinClassCommand>
{
    public JoinClassCommandValidator()
    {
        RuleFor(x => x.InviteCode)
            .NotEmpty()
            .WithMessage("Invite code is required")
            .Length(6)
            .WithMessage("Invite code must be exactly 6 characters")
            .Matches(@"^[A-Za-z0-9]{6}$")
            .WithMessage("Invite code must be alphanumeric");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
