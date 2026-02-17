namespace FrogEdu.Class.Application.Dtos;

public sealed record ClassSummaryResponse(
    Guid Id,
    string Name,
    string Grade,
    string InviteCode,
    int MaxStudents,
    string? BannerUrl,
    bool IsActive,
    Guid TeacherId,
    DateTime CreatedAt,
    int StudentCount,
    int AssignmentCount
);
