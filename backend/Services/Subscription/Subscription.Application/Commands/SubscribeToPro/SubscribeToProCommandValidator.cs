using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.SubscribeToPro;

/// <summary>
/// Validator for SubscribeToProCommand
/// </summary>
public sealed class SubscribeToProCommandValidator : AbstractValidator<SubscribeToProCommand>
{
    public SubscribeToProCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
