using System.Diagnostics;
using FrogEdu.Subscription.Application.Interfaces;
using FrogEdu.Subscription.Application.Queries.CheckDatabaseHealth;
using FrogEdu.Subscription.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Services;

/// <summary>
/// Service for checking database health
/// </summary>
public sealed class DatabaseHealthService : IDatabaseHealthService
{
    private readonly SubscriptionDbContext _dbContext;

    public DatabaseHealthService(SubscriptionDbContext dbContext)
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
            await _dbContext.Subscriptions.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

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
