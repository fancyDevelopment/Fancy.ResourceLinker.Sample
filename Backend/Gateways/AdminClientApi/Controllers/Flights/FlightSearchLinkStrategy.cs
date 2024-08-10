using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Flights;

public class FlightSearchLinkStrategy : LinkStrategyBase<FlightSearchViewModel>
{
    protected override void LinkResourceInternal(FlightSearchViewModel resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetSearchViewModel(resource.From, resource.To)));
        resource.AddLink("flightSearchVm", urlHelper.LinkTo<FlightsController>(c => c.GetSearchViewModel(null, null)));
        resource.AddLink("createFlightVm", urlHelper.LinkTo<FlightsController>(c => c.GetCreateViewModel()));

        foreach (dynamic flight in resource.Flights)
        {
            int flightId = (int)flight.Id;
            flight.AddLink("flightEditVm", urlHelper.LinkTo<FlightsController>(c => c.GetEditViewModel(flightId)));
        }
    }
}