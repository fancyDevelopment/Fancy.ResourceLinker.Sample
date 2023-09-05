using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Controllers;
using FlightShopping.Domain;
using FlightShopping.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FlightShopping.LinkStrategies;

public class ShoppingBasketLinkStrategy : LinkStrategyBase<ShoppingBasket>
{
    protected override void LinkResourceInternal(ShoppingBasket resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<ShoppingBasketController>(c => c.GetCurrentShoppingBasket()));
        resource.PaymentDetails.AddAction("update", "PUT", urlHelper.LinkTo<ShoppingBasketController>(c => c.UpdatePaymentDetails(null!)));

        if(resource.State == ShoppingBasketState.Current)
        {
            CheckoutDto checkout = new CheckoutDto(resource.Id);
            checkout.AddAction("execute", "POST", urlHelper.LinkTo<ShoppingBasketController>(c => c.Checkout(null!)));
            resource.Add("Checkout", checkout);
        }
    }
}