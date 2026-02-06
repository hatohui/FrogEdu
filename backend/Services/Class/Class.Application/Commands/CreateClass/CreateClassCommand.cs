using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.CreateClass;

public sealed record CreateClassCommand(
    string Name,
    string Description,
    string Grade,
    int MaxStudents,
    string UserId,
    string? BannerUrl = null
) : IRequest<Result<CreateClassResponse>>;
