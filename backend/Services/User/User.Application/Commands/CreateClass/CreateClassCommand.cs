using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateClass;

/// <summary>
/// Command to create a new class
/// </summary>
public sealed record CreateClassCommand(
    string Name,
    string? Subject,
    short Grade,
    Guid TeacherId,
    string? School,
    string? Description,
    int? MaxStudents
) : IRequest<Result<Guid>>;
