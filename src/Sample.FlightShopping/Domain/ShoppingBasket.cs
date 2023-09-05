using Fancy.ResourceLinker.Models;

namespace FlightShopping.Domain;

public enum ShoppingBasketState
{
    Current, 
    Ordered,
    BookingsConfirmed,
    PaymentCompleted
}

public class ShoppingBasket : ResourceBase
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string BookingCode { get; set; }

    public ShoppingBasketState State { get; set; }

    public ShoppingBasketPaymentDetails PaymentDetails { get; set; }

    public List<Ticket>? Tickets { get; }

    public float TotalBasketPrice { get; set; }

    private ShoppingBasket() 
    {
        UserId = null!;
        BookingCode = null!;
    }

    public ShoppingBasket(string userId)
    {
        UserId = userId;
        BookingCode = string.Format("{0:X}", (userId + DateTime.Now.ToString()).GetHashCode());
        PaymentDetails = new ShoppingBasketPaymentDetails();
    }
}

public class ShoppingBasketPaymentDetails : ResourceBase
{
    public string CreditCardNumber { get; set; } = string.Empty;

    public string CredidCardCvc { get; set; } = string.Empty;

    public string CreditCardValidUntil { get; set; } = string.Empty;
}
