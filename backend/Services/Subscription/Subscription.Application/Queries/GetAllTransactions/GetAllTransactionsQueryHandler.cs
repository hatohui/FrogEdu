using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Domain.Enums;
using FrogEdu.Subscription.Domain.Repositories;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllTransactions;

public sealed class GetAllTransactionsQueryHandler
    : IRequestHandler<GetAllTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepository _repository;

    public GetAllTransactionsQueryHandler(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TransactionDto>> Handle(
        GetAllTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Domain.Entities.Transaction> transactions;

        if (!string.IsNullOrEmpty(request.PaymentStatus))
        {
            if (Enum.TryParse<PaymentStatus>(request.PaymentStatus, true, out var status))
            {
                transactions = await _repository.GetByStatusAsync(status, cancellationToken);
            }
            else
            {
                return new List<TransactionDto>();
            }
        }
        else if (!string.IsNullOrEmpty(request.PaymentProvider))
        {
            if (Enum.TryParse<PaymentProvider>(request.PaymentProvider, true, out var provider))
            {
                transactions = await _repository.GetByProviderAsync(provider, cancellationToken);
            }
            else
            {
                return new List<TransactionDto>();
            }
        }
        else
        {
            transactions = await _repository.GetAllAsync(cancellationToken);
        }

        return transactions
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                TransactionCode = t.TransactionCode,
                Amount = t.Amount.Amount,
                Currency = t.Amount.Currency,
                PaymentProvider = t.PaymentProvider.ToString(),
                PaymentStatus = t.PaymentStatus.ToString(),
                ProviderTransactionId = t.ProviderTransactionId,
                CreatedAt = t.CreatedAt,
                UserSubscriptionId = t.UserSubscriptionId,
            })
            .ToList();
    }
}
