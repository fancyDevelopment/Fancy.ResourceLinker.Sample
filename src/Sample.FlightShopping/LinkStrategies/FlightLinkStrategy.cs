using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Controllers;
using FlightShopping.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FlightShopping.LinkStrategies;

public class FlightLinkStrategy : LinkStrategyBase<Flight>
{
    protected override void LinkResourceInternal(Flight resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetFlightById(resource.Id)));
        resource.Price?.AddAction("update", "PUT", urlHelper.LinkTo<FlightsController>(c => c.UpdatePrice(resource.Id, null!)));
    }
}