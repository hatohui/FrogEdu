namespace FrogEdu.User.Application.DTOs;

public sealed record CreateUserDto(
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role
);
