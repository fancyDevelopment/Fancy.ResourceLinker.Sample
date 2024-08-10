using Fancy.ResourceLinker.Models;

namespace FlightShopping.Domain;

public class Flight : ResourceBase
{
    public int Id { get; set; }

    public int AircraftId { get; set; }

    public FlightPrice? Price { get; set; }
}

public class FlightPrice : ResourceBase
{
    public float BasePrice { get; set; }

    public float SeatReservationSurcharge { get; set; }
}
