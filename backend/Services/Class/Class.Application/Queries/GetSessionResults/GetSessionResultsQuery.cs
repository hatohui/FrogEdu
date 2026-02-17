using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetSessionResults;

public sealed record GetSessionResultsQuery(Guid ExamSessionId)
    : IRequest<ExamSessionResultsResponse?>;
