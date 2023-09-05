using Fancy.ResourceLinker.Hateoas;
using FlightManagement.Controllers;
using FlightManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.LinkStrategies;

public class CreateFlightDtoLinkStrategy : LinkStrategyBase<CreateFlightDto>
{
    protected override void LinkResourceInternal(CreateFlightDto resource, IUrlHelper urlHelper)
    {
        //Add links
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetNewFlightTemplate()));

        // Add actions
        resource.AddAction("create", "POST", urlHelper.LinkTo<FlightsController>(c => c.CreateFlight(null!)));
    }
}
