using FrogEdu.User.Application.DTOs;

namespace FrogEdu.User.Application.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyList<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Role?> GetRoleEntityByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task CreateRoleAsync(Domain.Entities.Role role, CancellationToken cancellationToken = default);
    Task UpdateRoleAsync(Domain.Entities.Role role, CancellationToken cancellationToken = default);
    Task DeleteRoleAsync(Domain.Entities.Role role, CancellationToken cancellationToken = default);
    Task<bool> HasUsersWithRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}
