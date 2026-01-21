using FluentValidation;
using FrogEdu.User.Domain.ValueObjects;

namespace FrogEdu.User.Application.Commands.JoinClass;

/// <summary>
/// Validator for JoinClassCommand
/// </summary>
public sealed class JoinClassCommandValidator : AbstractValidator<JoinClassCommand>
{
    public JoinClassCommandValidator()
    {
        RuleFor(x => x.InviteCode)
            .NotEmpty()
            .WithMessage("Invite code is required")
            .Length(6)
            .WithMessage("Invite code must be exactly 6 characters")
            .Must(code => InviteCode.IsValidFormat(code))
            .WithMessage("Invite code contains invalid characters");

        RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student ID is required");
    }
}
