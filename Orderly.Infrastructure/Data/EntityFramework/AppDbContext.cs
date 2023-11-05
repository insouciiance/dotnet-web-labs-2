using Microsoft.EntityFrameworkCore;
using Orderly.Application.Entities;
using Orderly.Infrastructure.Data.EntityFramework.Config;

namespace Orderly.Infrastructure.Data.EntityFramework;

internal class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
 
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsersConfiguration());
        modelBuilder.ApplyConfiguration(new TicketsConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
