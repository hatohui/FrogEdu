using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetAllUsers;

public sealed record GetAllUsersQuery() : IRequest<IReadOnlyList<UserDto>>;
