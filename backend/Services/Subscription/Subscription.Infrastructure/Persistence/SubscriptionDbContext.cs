using Microsoft.EntityFrameworkCore;

namespace FrogEdu.Subscription.Infrastructure.Persistence;

/// <summary>
/// Database context for Subscription service
/// </summary>
public class SubscriptionDbContext : DbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity configurations will be added here
    }
}
