using FlightShopping.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlightShopping.Infrastructure;

public class FlightShoppingDbContext : DbContext
{
    public FlightShoppingDbContext(DbContextOptions<FlightShoppingDbContext> options) : base(options)
    {
    }

    public DbSet<Flight> Flights { get; set; } = null!;

    public DbSet<ShoppingBasket> ShoppingBaskets { get; set;} = null!;

    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Flight>().Property(f => f.Id).ValueGeneratedNever();
        modelBuilder.Entity<Flight>().OwnsOne(f => f.Price);
        modelBuilder.Entity<ShoppingBasket>().OwnsOne(sb => sb.PaymentDetails);

        modelBuilder.Entity<Flight>().HasData(
            new { Id = 1, AircraftId = 1 });

        modelBuilder.Entity<Flight>().OwnsOne(f => f.Price).HasData(new { FlightId = 1, BasePrice = 100.0f, SeatReservationSurcharge = 10.0f, PremiumSurcharge = 50.0f, BusinessSurcharge = 100.0f});
    }
}