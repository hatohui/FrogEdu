using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserEntity = FrogEdu.User.Domain.Entities.User;

namespace FrogEdu.User.Infrastructure.Persistence;

/// <summary>
/// Seeds the database with initial data
/// </summary>
public class DatabaseSeeder
{
    private readonly UserDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(UserDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seed user from Cognito if they don't exist in the database
    /// </summary>
    public async Task SeedUserFromCognitoAsync(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        UserRole role = UserRole.Student
    )
    {
        try
        {
            // Check if user already exists
            var cognitoIdVO = CognitoUserId.Create(cognitoId);
            var existingUser = await _context
                .Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.CognitoId == cognitoIdVO);

            if (existingUser != null)
            {
                _logger.LogInformation(
                    "User with Cognito ID {CognitoId} already exists in database with ID {UserId}",
                    cognitoId,
                    existingUser.Id
                );
                return;
            }

            // Create new user
            var newUser = UserEntity.Create(cognitoId, email, firstName, lastName, role);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Successfully seeded user: {Email} (Cognito ID: {CognitoId}, DB ID: {UserId})",
                email,
                cognitoId,
                newUser.Id
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to seed user with Cognito ID {CognitoId}", cognitoId);
            throw;
        }
    }

    /// <summary>
    /// Seed multiple users from Cognito
    /// </summary>
    public async Task SeedUsersAsync(
        IEnumerable<(
            string CognitoId,
            string Email,
            string FirstName,
            string LastName,
            UserRole Role
        )> users
    )
    {
        foreach (var (cognitoId, email, firstName, lastName, role) in users)
        {
            await SeedUserFromCognitoAsync(cognitoId, email, firstName, lastName, role);
        }
    }
}
