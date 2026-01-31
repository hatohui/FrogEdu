using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FrogEdu.User.Infrastructure.Persistence;

public class RoleDbContext(DbContextOptions<RoleDbContext> options)
    : DbContext(options),
        IUnitOfWork
{
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}
