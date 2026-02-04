using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrices;

public sealed record GetMatricesQuery(string UserId) : IRequest<GetMatricesResponse>;
