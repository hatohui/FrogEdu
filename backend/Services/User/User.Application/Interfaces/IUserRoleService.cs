using FrogEdu.User.Application.DTOs;

namespace FrogEdu.User.Application.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyList<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
