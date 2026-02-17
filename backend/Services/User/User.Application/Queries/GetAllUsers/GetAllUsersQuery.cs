using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? Role = null,
    string? SortBy = "createdAt",
    string? SortOrder = "desc"
) : IRequest<PaginatedUsersResponse>;

public sealed record PaginatedUsersResponse(
    IReadOnlyList<UserDto> Items,
    int Total,
    int Page,
    int PageSize,
    int TotalPages
);
