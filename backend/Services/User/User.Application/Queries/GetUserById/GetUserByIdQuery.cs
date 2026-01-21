using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserById;

/// <summary>
/// Query to get user by ID
/// </summary>
public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;
