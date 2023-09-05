using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Flights;

public class FlightSearchVmLinkStrategy : LinkStrategyBase<FlightSearchVm>
{
    protected override void LinkResourceInternal(FlightSearchVm resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetSearchVm(resource.From, resource.To)));
        resource.AddLink("flightSearchVm", urlHelper.LinkTo<FlightsController>(c => c.GetSearchVm(null, null)));
        resource.AddLink("createFlightVm", urlHelper.LinkTo<FlightsController>(c => c.GetCreateVm()));

        foreach (dynamic flight in resource.Flights)
        {
            double flightId = flight.Id;
            flight.AddLink("flightEditVm", urlHelper.LinkTo<FlightsController>(c => c.GetEditVm(flightId)));
        }
    }
}