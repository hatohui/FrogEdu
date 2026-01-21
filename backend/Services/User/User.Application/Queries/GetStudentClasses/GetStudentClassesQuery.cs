using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetStudentClasses;

/// <summary>
/// Query to get all classes a student is enrolled in
/// </summary>
public sealed record GetStudentClassesQuery(Guid StudentId) : IRequest<IReadOnlyList<ClassDto>>;
