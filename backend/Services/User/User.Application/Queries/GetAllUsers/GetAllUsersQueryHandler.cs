using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IReadOnlyList<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users
            .Select(u => new UserDto(
                u.Id,
                u.CognitoId.Value,
                u.Email.Value,
                u.FirstName,
                u.LastName,
                u.RoleId,
                u.AvatarUrl,
                u.IsEmailVerified,
                u.CreatedAt,
                u.UpdatedAt
            ))
            .ToList();
    }
}
