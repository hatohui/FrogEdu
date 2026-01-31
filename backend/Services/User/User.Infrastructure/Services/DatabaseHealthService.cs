using System.Diagnostics;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Application.Queries.CheckDatabaseHealth;
using FrogEdu.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.User.Infrastructure.Services;

public sealed class DatabaseHealthService(UserDbContext dbContext) : IDatabaseHealthService
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<DatabaseHealthDto> CheckHealthAsync(
        CancellationToken cancellationToken = default
    )
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            stopwatch.Stop();

            return new DatabaseHealthDto(
                IsHealthy: true,
                Status: "Database connection healthy",
                ResponseTimeMs: stopwatch.ElapsedMilliseconds
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            return new DatabaseHealthDto(
                IsHealthy: false,
                Status: "Database connection failed",
                ResponseTimeMs: stopwatch.ElapsedMilliseconds,
                Error: ex.Message
            );
        }
    }
}
