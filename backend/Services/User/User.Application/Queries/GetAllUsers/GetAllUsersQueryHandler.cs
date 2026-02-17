using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository,
    ILogger<GetAllUsersQueryHandler> logger
) : IRequestHandler<GetAllUsersQuery, PaginatedUsersResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger = logger;

    public async Task<PaginatedUsersResponse> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        // Apply filtering
        var query = users.AsEnumerable();

        // Search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLowerInvariant();
            query = query.Where(u =>
                u.Email.Value.ToLowerInvariant().Contains(searchLower)
                || (u.FirstName != null && u.FirstName.ToLowerInvariant().Contains(searchLower))
                || (u.LastName != null && u.LastName.ToLowerInvariant().Contains(searchLower))
            );
        }

        // Role filter
        if (!string.IsNullOrWhiteSpace(request.Role) && request.Role.ToLowerInvariant() != "all")
        {
            query = query.Where(u =>
            {
                var userRole = RoleConstants.MapRoleIdToName(u.RoleId);
                return userRole.Equals(request.Role, StringComparison.OrdinalIgnoreCase);
            });
        }

        // Sorting
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "email" => request.SortOrder == "asc"
                ? query.OrderBy(u => u.Email.Value)
                : query.OrderByDescending(u => u.Email.Value),
            "firstname" => request.SortOrder == "asc"
                ? query.OrderBy(u => u.FirstName)
                : query.OrderByDescending(u => u.FirstName),
            "lastname" => request.SortOrder == "asc"
                ? query.OrderBy(u => u.LastName)
                : query.OrderByDescending(u => u.LastName),
            "createdat" or _ => request.SortOrder == "asc"
                ? query.OrderBy(u => u.CreatedAt)
                : query.OrderByDescending(u => u.CreatedAt),
        };

        var total = query.Count();
        var totalPages = (int)Math.Ceiling(total / (double)request.PageSize);

        // Pagination
        var paginatedUsers = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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

        _logger.LogInformation(
            "Retrieved {Count} users (page {Page}/{TotalPages})",
            paginatedUsers.Count,
            request.Page,
            totalPages
        );

        return new PaginatedUsersResponse(
            paginatedUsers,
            total,
            request.Page,
            request.PageSize,
            totalPages
        );
    }
}
