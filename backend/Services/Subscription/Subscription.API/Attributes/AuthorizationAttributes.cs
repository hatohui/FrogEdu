using Microsoft.AspNetCore.Authorization;

namespace FrogEdu.Subscription.API.Attributes;

/// <summary>
/// Authorize with flexible role support. Admin users bypass all role checks.
/// </summary>
/// <example>
/// [AuthorizeRoles("Student")] - Only students
/// [AuthorizeRoles("Student", "Teacher")] - Students or Teachers
/// Admin users can access regardless of specified roles
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}

/// <summary>
/// Authorize only Teacher role (Admin bypasses)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeTeacherAttribute : AuthorizeAttribute
{
    public AuthorizeTeacherAttribute() => Roles = "Teacher,Admin";
}

/// <summary>
/// Authorize only Student role (Admin bypasses)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeStudentAttribute : AuthorizeAttribute
{
    public AuthorizeStudentAttribute() => Roles = "Student,Admin";
}

/// <summary>
/// Authorize only Admin role
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAdminAttribute : AuthorizeAttribute
{
    public AuthorizeAdminAttribute() => Roles = "Admin";
}
