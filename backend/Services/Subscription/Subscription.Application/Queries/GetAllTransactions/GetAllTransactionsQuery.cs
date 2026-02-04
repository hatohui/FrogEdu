using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetAllTransactions;

public sealed record GetAllTransactionsQuery(
    string? PaymentStatus = null,
    string? PaymentProvider = null
) : IRequest<IReadOnlyList<TransactionDto>>;
