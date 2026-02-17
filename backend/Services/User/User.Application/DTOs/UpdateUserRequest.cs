namespace FrogEdu.User.Application.DTOs;

public sealed record UpdateUserRequest(string? FirstName, string? LastName, Guid? RoleId);
