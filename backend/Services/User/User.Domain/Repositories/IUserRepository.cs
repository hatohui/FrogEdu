using FrogEdu.User.Domain.Entities;

namespace FrogEdu.User.Domain.Repositories;

/// <summary>
/// Repository interface for User aggregate
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get user by ID
    /// </summary>
    Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user by Cognito ID
    /// </summary>
    Task<Entities.User?> GetByCognitoIdAsync(
        string cognitoId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get user by email
    /// </summary>
    Task<Entities.User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check if email exists
    /// </summary>
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new user
    /// </summary>
    Task AddAsync(Entities.User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing user
    /// </summary>
    void Update(Entities.User user);

    /// <summary>
    /// Delete user (soft delete)
    /// </summary>
    void Delete(Entities.User user);

    /// <summary>
    /// Save changes
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
