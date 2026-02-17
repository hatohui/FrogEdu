namespace FrogEdu.Class.Application.Interfaces;

public interface IUserServiceClient
{
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserDto>> GetUsersByIdsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default
    );
}

public sealed record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? AvatarUrl
);
