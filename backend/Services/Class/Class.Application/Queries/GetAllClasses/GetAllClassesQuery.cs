using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAllClasses;

/// <summary>
/// Admin query to get all classes in the system
/// </summary>
public sealed record GetAllClassesQuery : IRequest<IReadOnlyList<ClassSummaryResponse>>;
