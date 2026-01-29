using System.Diagnostics;
using FrogEdu.Exam.Application.Interfaces;
using FrogEdu.Exam.Application.Queries.CheckDatabaseHealth;
using FrogEdu.Exam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Exam.Infrastructure.Services;

/// <summary>
/// Service for checking database health
/// </summary>
public sealed class DatabaseHealthService : IDatabaseHealthService
{
    private readonly ExamDbContext _dbContext;

    public DatabaseHealthService(ExamDbContext dbContext)
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
            await _dbContext.Database.CanConnectAsync(cancellationToken);

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
