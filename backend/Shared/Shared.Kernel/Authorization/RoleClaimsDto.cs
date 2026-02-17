namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// DTO representing the user's role claims fetched from the User microservice.
/// </summary>
public sealed record RoleClaimsDto
{
    /// <summary>
    /// The user's Cognito sub (ID)
    /// </summary>
    public string CognitoSub { get; init; } = string.Empty;

    /// <summary>
    /// The user's internal database ID
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// The user's role ID
    /// </summary>
    public Guid RoleId { get; init; }

    /// <summary>
    /// The user's role name (Admin, Teacher, Student)
    /// </summary>
    public string Role { get; init; } = RoleConstants.Student;

    /// <summary>
    /// Create a default Student role claims DTO
    /// </summary>
    public static RoleClaimsDto Default(string cognitoSub) =>
        new() { CognitoSub = cognitoSub, Role = RoleConstants.Student };
}

/// <summary>
/// Well-known role constants used across all microservices.
/// </summary>
public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Teacher = "Teacher";
    public const string Student = "Student";

    // Well-known Role IDs from User service seed data
    public static readonly Guid AdminRoleId = new("c3333333-3333-3333-3333-333333333333");
    public static readonly Guid TeacherRoleId = new("a1111111-1111-1111-1111-111111111111");
    public static readonly Guid StudentRoleId = new("b2222222-2222-2222-2222-222222222222");

    /// <summary>
    /// Map a role GUID to its name
    /// </summary>
    public static string MapRoleIdToName(Guid roleId)
    {
        if (roleId == AdminRoleId)
            return Admin;
        if (roleId == TeacherRoleId)
            return Teacher;
        if (roleId == StudentRoleId)
            return Student;
        return Student;
    }
}
