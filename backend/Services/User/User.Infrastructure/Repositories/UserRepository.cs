using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using UserEntity = FrogEdu.User.Domain.Entities.User;

namespace FrogEdu.User.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IUserRepository
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<UserEntity?> GetByCognitoIdAsync(
        string cognitoId,
        CancellationToken cancellationToken = default
    )
    {
        // Use EF.Property to access the converted value directly for better query translation
        return await _context.Users.FirstOrDefaultAsync(
            u => EF.Property<string>(u.CognitoId, "Value") == cognitoId,
            cancellationToken
        );
    }

    public async Task<UserEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(
                u => EF.Property<string>(u.Email, "Value") == email,
                cancellationToken
            );
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context
            .Users.AsNoTracking()
            .AnyAsync(u => EF.Property<string>(u.Email, "Value") == email, cancellationToken);
    }

    public async Task AddAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public void Update(UserEntity user)
    {
        _context.Users.Update(user);
    }

    public void Delete(UserEntity user)
    {
        // Soft delete
        user.GetType()
            .GetMethod(
                "SoftDelete",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            )
            ?.Invoke(user, null);
        _context.Users.Update(user);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
