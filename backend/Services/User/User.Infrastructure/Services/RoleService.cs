using System.Linq;
using System.Threading.Tasks;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.User.Infrastructure.Services;

public sealed class RoleService(RoleDbContext dbContext) : IRoleService
{
    private readonly RoleDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<RoleDto>> GetAllRolesAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .Roles.AsNoTracking()
            .Select(r => new RoleDto(r.Id, r.Name.ToString(), r.Description))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<RoleDto?> GetRoleByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        if (!Enum.TryParse<UserRole>(name, true, out var parsed))
            return null;

        var role = await _dbContext
            .Roles.AsNoTracking()
            .Where(r => r.Name == parsed)
            .Select(r => new RoleDto(r.Id, r.Name.ToString(), r.Description))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return role;
    }

    public async Task<RoleDto?> GetRoleByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var role = await _dbContext
            .Roles.AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RoleDto(r.Id, r.Name.ToString(), r.Description))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return role;
    }
}
