using Fancy.ResourceLinker.Models;

namespace FlightShopping.Dtos;

public class CheckoutDto : ResourceBase
{
    public int ShoppingBasketId { get; set; }

    public CheckoutDto(int shoppingBasketId)
    {
        ShoppingBasketId = shoppingBasketId;
    }
}