using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetClassDetails;

/// <summary>
/// Query to get class details including roster
/// </summary>
public sealed record GetClassDetailsQuery(Guid ClassId, Guid RequesterId)
    : IRequest<Result<ClassDetailsDto>>;
