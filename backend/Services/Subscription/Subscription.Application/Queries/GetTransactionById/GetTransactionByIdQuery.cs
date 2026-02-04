using FrogEdu.Subscription.Application.DTOs;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto?>;
