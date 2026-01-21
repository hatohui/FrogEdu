namespace FrogEdu.User.Application.DTOs;

/// <summary>
/// DTO for creating a user from Cognito webhook
/// </summary>
public sealed record CreateUserDto(
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role
);
