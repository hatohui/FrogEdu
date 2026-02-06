namespace FrogEdu.Class.Application.Dtos;

public sealed record CreateClassResponse(
    Guid ClassId,
    string Name,
    string Grade,
    string InviteCode,
    int MaxStudents,
    string? BannerUrl,
    bool IsActive,
    Guid TeacherId,
    DateTime CreatedAt
);
