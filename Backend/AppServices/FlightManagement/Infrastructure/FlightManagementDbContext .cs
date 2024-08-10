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

        modelBuilder.Entity<Flight>().HasData(new object[]
        {
            new { Id = 1 },
            new { Id = 2 }
        });

        modelBuilder.Entity<Flight>().OwnsOne(f => f.Connection).HasData(new[]
        {
            new { FlightId = 1, From = "Munich", To = "Hamburg", IcaoFrom = "EDDM", IcaoTo = "EDDH" },
            new { FlightId = 2, From = "Berlin", To = "Frankfurt", IcaoFrom = "EDDB", IcaoTo = "EDDF" }
        });
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Times).HasData(new[]
        {
            new { FlightId = 1, TakeOff = DateTime.Now.AddDays(1), Landing = DateTime.Now.AddDays(1).AddHours(1) },
            new { FlightId = 2, TakeOff = DateTime.Now.AddDays(1), Landing = DateTime.Now.AddDays(1).AddHours(1) }
        });
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Operator).HasData(new[]
        {
            new { FlightId = 1, SeatCount = 60, Name = "Lufthansa", ShortName = "LH" },
            new { FlightId = 2, SeatCount = 90, Name = "Lufthansa", ShortName = "LH" }
        });
    }
}