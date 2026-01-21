namespace FrogEdu.User.Application.DTOs;

/// <summary>
/// DTO for class information
/// </summary>
public record ClassDto(
    Guid Id,
    string Name,
    string? Subject,
    short Grade,
    Guid HomeroomTeacherId,
    string? TeacherName,
    string? School,
    string? Description,
    int? MaxStudents,
    int StudentCount,
    string? InviteCode,
    DateTime? InviteCodeExpiresAt,
    bool IsArchived,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

/// <summary>
/// DTO for creating a new class
/// </summary>
public record CreateClassDto(
    string Name,
    string? Subject,
    short Grade,
    string? School,
    string? Description,
    int? MaxStudents
);

/// <summary>
/// DTO for updating a class
/// </summary>
public record UpdateClassDto(
    string Name,
    string? Subject,
    short Grade,
    string? School,
    string? Description,
    int? MaxStudents
);

/// <summary>
/// DTO for joining a class via invite code
/// </summary>
public record JoinClassDto(string InviteCode);

/// <summary>
/// DTO for class member (student or teacher)
/// </summary>
public record ClassMemberDto(
    Guid UserId,
    string FullName,
    string? AvatarUrl,
    string Role,
    DateTime JoinedAt
);

/// <summary>
/// DTO for class details including roster
/// </summary>
public record ClassDetailsDto(
    Guid Id,
    string Name,
    string? Subject,
    short Grade,
    Guid HomeroomTeacherId,
    string? TeacherName,
    string? School,
    string? Description,
    int? MaxStudents,
    string? InviteCode,
    DateTime? InviteCodeExpiresAt,
    bool IsArchived,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<ClassMemberDto> Members
);

/// <summary>
/// Summary DTO for dashboard stats
/// </summary>
public record DashboardStatsDto(
    int ClassCount,
    int StudentCount,
    int ExamCount,
    int ContentItemCount
);
