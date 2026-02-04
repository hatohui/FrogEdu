using FrogEdu.Shared.Kernel;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.UpdateTransactionStatus;

public sealed class UpdateTransactionStatusCommandHandler
    : IRequestHandler<UpdateTransactionStatusCommand, Result>
{
    private readonly ITransactionRepository _repository;

    public UpdateTransactionStatusCommandHandler(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        UpdateTransactionStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transaction is null)
        {
            return Result.Failure("Transaction not found");
        }

        if (!Enum.TryParse<PaymentStatus>(request.PaymentStatus, true, out var status))
        {
            return Result.Failure(
                $"Invalid payment status: {request.PaymentStatus}. Valid values: Pending, Paid, Failed, Cancelled"
            );
        }

        try
        {
            transaction.UpdateStatus(status, request.ProviderTransactionId);

            _repository.Update(transaction);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
