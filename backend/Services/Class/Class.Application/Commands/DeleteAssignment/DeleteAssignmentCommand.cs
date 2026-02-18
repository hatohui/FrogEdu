using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.DeleteAssignment;

public sealed record DeleteAssignmentCommand(
    Guid ClassId,
    Guid AssignmentId,
    string UserId,
    string Role = "Teacher"
) : IRequest<Result<bool>>;
