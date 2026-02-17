using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetMyClasses;

public sealed record GetMyClassesQuery(string UserId, string Role)
    : IRequest<IReadOnlyList<ClassSummaryResponse>>;
