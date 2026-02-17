namespace FrogEdu.Class.Application.Dtos;

public sealed record ClassDetailResponse(
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
    IReadOnlyList<EnrollmentWithUserDto> Enrollments,
    IReadOnlyList<AssignmentResponse> Assignments
);

public sealed record EnrollmentDto(Guid Id, Guid StudentId, DateTime JoinedAt, string Status);
