using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.AwardBadge;

public sealed record AwardBadgeCommand(
    Guid StudentId,
    Guid BadgeId,
    Guid ClassId,
    Guid? ExamSessionId,
    Guid TeacherId,
    string? CustomPraise
) : IRequest<Result<Guid>>;
