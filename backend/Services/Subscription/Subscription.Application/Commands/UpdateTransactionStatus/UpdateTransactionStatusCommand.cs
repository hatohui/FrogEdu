using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.UpdateTransactionStatus;

public sealed record UpdateTransactionStatusCommand(
    Guid TransactionId,
    string PaymentStatus,
    string? ProviderTransactionId
) : IRequest<Result>;
