using FrogEdu.User.Domain.Entities;

namespace FrogEdu.User.Domain.Repositories;

public interface IUserRepository
{
    Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Entities.User?> GetByCognitoIdAsync(
        string cognitoId,
        CancellationToken cancellationToken = default
    );
    Task<Entities.User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );
    Task<Entities.User?> GetByVerificationTokenAsync(
        string token,
        CancellationToken cancellationToken = default
    );
    Task<Entities.User?> GetByPasswordResetTokenAsync(
        string token,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Entities.User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.User user, CancellationToken cancellationToken = default);
    void Update(Entities.User user);
    void Delete(Entities.User user);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
