# Authorization Guide

## Overview

The FrogEdu authorization system supports flexible role-based access control with automatic Admin bypass.

## How It Works

- **Admin users bypass all role checks** - they can access any endpoint regardless of role restrictions
- Multiple roles can be specified for endpoints that should be accessible to different user types
- Single roles restrict access to only that role (plus Admin)

## Available Attributes

### 1. `[AuthorizeRoles(...)]` - Flexible Multi-Role Authorization

The most flexible option - specify one or more roles. Admin always has access.

**Examples:**

```csharp
// Only students (+ Admin bypass)
[AuthorizeRoles("Student")]
public IActionResult StudentOnly() { }

// Students OR Teachers (+ Admin bypass)
[AuthorizeRoles("Student", "Teacher")]
public IActionResult StudentOrTeacher() { }

// Only Teachers (+ Admin bypass)
[AuthorizeRoles("Teacher")]
public IActionResult TeacherOnly() { }
```

### 2. `[AuthorizeTeacher]` - Teacher + Admin Only

Shorthand for `[AuthorizeRoles("Teacher")]`

```csharp
[AuthorizeTeacher]
public IActionResult TeacherEndpoint() { }
// Accessible by: Teacher, Admin
```

### 3. `[AuthorizeStudent]` - Student + Admin Only

Shorthand for `[AuthorizeRoles("Student")]`

```csharp
[AuthorizeStudent]
public IActionResult StudentEndpoint() { }
// Accessible by: Student, Admin
```

### 4. `[AuthorizeAdmin]` - Admin Only

Restricts access to Admin users only.

```csharp
[AuthorizeAdmin]
public IActionResult AdminEndpoint() { }
// Accessible by: Admin ONLY
```

## Real-World Examples

### User Profile Endpoints

```csharp
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    // Any authenticated user can view their own profile
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser() { }

    // Students and Teachers can update profiles (Admin can too)
    [HttpPut("me")]
    [AuthorizeRoles("Student", "Teacher")]
    public IActionResult UpdateProfile() { }

    // Only admins can delete users
    [HttpDelete("{id}")]
    [AuthorizeAdmin]
    public IActionResult DeleteUser(Guid id) { }
}
```

### Class Management Endpoints

```csharp
[ApiController]
[Route("api/classes")]
public class ClassController : ControllerBase
{
    // Only teachers can create classes (Admin can too)
    [HttpPost]
    [AuthorizeTeacher]
    public IActionResult CreateClass() { }

    // Students and Teachers can view classes
    [HttpGet]
    [AuthorizeRoles("Student", "Teacher")]
    public IActionResult GetClasses() { }

    // Only admins can delete classes
    [HttpDelete("{id}")]
    [AuthorizeAdmin]
    public IActionResult DeleteClass(Guid id) { }
}
```

## Role Claim Configuration

The system automatically maps Cognito's `custom:role` claim to the standard `Role` claim during token validation. This happens in `AuthenticationExtensions.cs`:

```csharp
OnTokenValidated = context =>
{
    var customRole = context.Principal?.FindFirst("custom:role")?.Value;
    if (!string.IsNullOrEmpty(customRole))
    {
        identity.AddClaim(new Claim(ClaimTypes.Role, customRole));
    }
    return Task.CompletedTask;
}
```

## Testing Authorization

### Test with different roles:

```bash
# Student token
curl -H "Authorization: Bearer <student_token>" http://localhost:5000/api/users/me

# Teacher token
curl -H "Authorization: Bearer <teacher_token>" http://localhost:5000/api/classes

# Admin token (bypasses all role checks)
curl -H "Authorization: Bearer <admin_token>" http://localhost:5000/api/admin/users
```

## Migration from Policy-Based to Role-Based

**Old approach (policies):**

```csharp
[Authorize(Policy = "TeacherOnly")]
public IActionResult OldWay() { }
```

**New approach (roles):**

```csharp
[AuthorizeTeacher]
// or
[AuthorizeRoles("Teacher")]
public IActionResult NewWay() { }
```

Both approaches work, but role-based is more flexible and allows Admin bypass.
