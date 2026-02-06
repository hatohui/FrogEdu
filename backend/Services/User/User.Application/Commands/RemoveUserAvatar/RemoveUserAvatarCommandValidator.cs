using FluentValidation;

namespace FrogEdu.User.Application.Commands.RemoveUserAvatar;

public sealed class RemoveUserAvatarCommandValidator : AbstractValidator<RemoveUserAvatarCommand>
{
    public RemoveUserAvatarCommandValidator()
    {
        RuleFor(x => x.CognitoId).NotEmpty().WithMessage("Cognito ID is required");
    }
}
