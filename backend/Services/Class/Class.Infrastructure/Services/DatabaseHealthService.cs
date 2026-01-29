using System.Diagnostics;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Application.Queries.CheckDatabaseHealth;
using FrogEdu.Class.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Class.Infrastructure.Services;

/// <summary>
/// Service for checking database health
/// </summary>
public sealed class DatabaseHealthService : IDatabaseHealthService
{
    private readonly ClassDbContext _dbContext;

    public DatabaseHealthService(ClassDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DatabaseHealthDto> CheckHealthAsync(
        CancellationToken cancellationToken = default
    )
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Attempt a simple query to verify connection
            await _dbContext.Classes.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

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
