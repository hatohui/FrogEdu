namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Unit of Work interface for managing database transactions
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Save all changes made in the current transaction
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
