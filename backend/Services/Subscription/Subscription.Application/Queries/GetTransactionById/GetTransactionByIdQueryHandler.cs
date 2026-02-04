using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetTransactionById;

public sealed class GetTransactionByIdQueryHandler
    : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>
{
    private readonly ITransactionRepository _repository;

    public GetTransactionByIdQueryHandler(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<TransactionDto?> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction is null)
            return null;

        return new TransactionDto
        {
            Id = transaction.Id,
            TransactionCode = transaction.TransactionCode,
            Amount = transaction.Amount.Amount,
            Currency = transaction.Amount.Currency,
            PaymentProvider = transaction.PaymentProvider.ToString(),
            PaymentStatus = transaction.PaymentStatus.ToString(),
            ProviderTransactionId = transaction.ProviderTransactionId,
            CreatedAt = transaction.CreatedAt,
            UserSubscriptionId = transaction.UserSubscriptionId,
        };
    }
}
