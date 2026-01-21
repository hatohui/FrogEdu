using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetTeacherClasses;

/// <summary>
/// Query to get all classes for a teacher
/// </summary>
public sealed record GetTeacherClassesQuery(Guid TeacherId, bool IncludeArchived = false)
    : IRequest<IReadOnlyList<ClassDto>>;
