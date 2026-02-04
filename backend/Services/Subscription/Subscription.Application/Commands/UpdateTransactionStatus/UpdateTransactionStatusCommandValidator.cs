using FluentValidation;

namespace FrogEdu.Subscription.Application.Commands.UpdateTransactionStatus;

public sealed class UpdateTransactionStatusCommandValidator
    : AbstractValidator<UpdateTransactionStatusCommand>
{
    public UpdateTransactionStatusCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty().WithMessage("Transaction ID is required");

        RuleFor(x => x.PaymentStatus)
            .NotEmpty()
            .WithMessage("Payment status is required")
            .Must(status =>
                status.Equals("Pending", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Paid", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Failed", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)
            )
            .WithMessage("Payment status must be Pending, Paid, Failed, or Cancelled");
    }
}
