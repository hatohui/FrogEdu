using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetExamSessions;

public sealed record GetExamSessionsQuery(Guid ClassId)
    : IRequest<IReadOnlyList<ExamSessionResponse>>;
