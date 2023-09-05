using FlightManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Infrastructure;

public class FlightManagementDbContext : DbContext
{
    public FlightManagementDbContext(DbContextOptions<FlightManagementDbContext> options) : base(options) { }

    public DbSet<Flight> Flights { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Connection).Property(c => c.IcaoFrom).HasMaxLength(4);
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Connection).Property(c => c.IcaoTo).HasMaxLength(4);
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Times);
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Operator);
    }
}