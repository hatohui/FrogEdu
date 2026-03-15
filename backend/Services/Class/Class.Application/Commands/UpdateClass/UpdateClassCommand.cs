using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.UpdateClass;

public sealed record UpdateClassCommand(
    Guid ClassId,
    string Name,
    string Grade,
    int MaxStudents,
    string UserId,
    string? BannerUrl = null
) : IRequest<Result>;
