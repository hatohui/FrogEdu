using FrogEdu.User.Domain.Entities;

namespace FrogEdu.User.Domain.Repositories;

/// <summary>
/// Repository interface for Class aggregate
/// </summary>
public interface IClassRepository
{
    /// <summary>
    /// Get class by ID with enrollments
    /// </summary>
    Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get class by ID including enrollments and user details
    /// </summary>
    Task<Class?> GetByIdWithEnrollmentsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get class by invite code
    /// </summary>
    Task<Class?> GetByInviteCodeAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get all classes for a teacher
    /// </summary>
    Task<IReadOnlyList<Class>> GetByTeacherIdAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get all classes a student is enrolled in
    /// </summary>
    Task<IReadOnlyList<Class>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check if invite code exists and is valid
    /// </summary>
    Task<bool> InviteCodeExistsAsync(
        string inviteCode,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Add new class
    /// </summary>
    Task AddAsync(Class classEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing class
    /// </summary>
    void Update(Class classEntity);

    /// <summary>
    /// Delete class (soft delete)
    /// </summary>
    void Delete(Class classEntity);

    /// <summary>
    /// Save changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
