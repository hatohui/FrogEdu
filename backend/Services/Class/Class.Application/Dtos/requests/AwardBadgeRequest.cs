namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record AwardBadgeRequest(
    Guid StudentId,
    Guid BadgeId,
    Guid ClassId,
    Guid? ExamSessionId = null,
    string? CustomPraise = null
);
