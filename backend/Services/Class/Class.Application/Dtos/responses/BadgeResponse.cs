namespace FrogEdu.Class.Application.Dtos.responses;

public sealed record BadgeDto(
    Guid Id,
    string Name,
    string Description,
    string IconUrl,
    string BadgeType,
    int RequiredScore,
    bool IsActive
);

public sealed record StudentBadgeDto(
    Guid Id,
    Guid StudentId,
    Guid BadgeId,
    Guid ClassId,
    Guid? ExamSessionId,
    Guid? AwardedByTeacherId,
    string? CustomPraise,
    DateTime AwardedAt,
    string? BadgeName,
    string? BadgeIconUrl,
    string? BadgeType
);
