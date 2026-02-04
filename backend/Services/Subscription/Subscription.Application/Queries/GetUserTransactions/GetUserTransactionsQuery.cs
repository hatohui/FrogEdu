using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetUserTransactions;

/// <summary>
/// Query to get all transactions for a user
/// </summary>
public sealed record GetUserTransactionsQuery(Guid UserId)
    : IRequest<IReadOnlyList<TransactionDto>>;
