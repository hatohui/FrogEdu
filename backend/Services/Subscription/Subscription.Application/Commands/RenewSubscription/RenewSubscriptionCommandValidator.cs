using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.RenewSubscription;

public sealed class RenewSubscriptionCommandValidator : AbstractValidator<RenewSubscriptionCommand>
{
    public RenewSubscriptionCommandValidator()
    {
        RuleFor(x => x.SubscriptionId).NotEmpty().WithMessage("Subscription ID is required");

        RuleFor(x => x.NewEndDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("New end date must be in the future");
    }
}
