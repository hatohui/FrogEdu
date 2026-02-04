using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.CancelSubscription;

/// <summary>
/// Validator for the CancelSubscriptionCommand
/// </summary>
public sealed class CancelSubscriptionCommandValidator
    : AbstractValidator<CancelSubscriptionCommand>
{
    public CancelSubscriptionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
    }
}
