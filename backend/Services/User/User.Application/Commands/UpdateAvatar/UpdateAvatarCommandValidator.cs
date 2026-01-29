using FluentValidation;

namespace FrogEdu.User.Application.Commands.UpdateAvatar;

/// <summary>
/// Validator for UpdateAvatarCommand
/// </summary>
public sealed class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    public UpdateAvatarCommandValidator()
    {
        RuleFor(x => x.CognitoId).NotEmpty().WithMessage("Cognito ID is required");

        RuleFor(x => x.AvatarUrl)
            .NotEmpty()
            .WithMessage("Avatar URL is required")
            .MaximumLength(500)
            .WithMessage("Avatar URL must not exceed 500 characters");
    }
}
