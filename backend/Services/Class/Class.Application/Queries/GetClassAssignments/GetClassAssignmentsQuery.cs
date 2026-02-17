using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassAssignments;

public sealed record GetClassAssignmentsQuery(Guid ClassId)
    : IRequest<IReadOnlyList<AssignmentResponse>>;
