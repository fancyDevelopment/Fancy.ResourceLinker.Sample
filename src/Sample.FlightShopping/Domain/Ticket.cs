using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightShopping.Domain;

public class Ticket
{
    public int Id { get; set; }

    public int? FlightBookingTicketId { get; set; }

    public int FlightId { get; set; }

    [JsonIgnore]
    [Required]
    public ShoppingBasket? Basket { get; set; }

    public float BasePrice { get; set; }

    public float SeatReservationSurcharge { get; set; }

    public float ClassSurcharge { get; set; }

    public float TotalTicketPrice { get; private set; }

    private Ticket() 
    {
    }

    public Ticket(int flightId, float basePrice, float seatReservationSurcharge, float classSurcharge)
    {
        FlightId = flightId;
        BasePrice = basePrice;
        SeatReservationSurcharge = seatReservationSurcharge;
        ClassSurcharge = classSurcharge;
        TotalTicketPrice = basePrice + SeatReservationSurcharge + ClassSurcharge;
    }
}