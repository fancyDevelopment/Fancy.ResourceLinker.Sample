using Microsoft.AspNetCore.Mvc;
using Fancy.ResourceLinker.Hateoas;

namespace Gateway.Controllers.Flights;

public class FlightEditLinkStrategy : LinkStrategyBase<FlightEditViewModel>
{
    protected override void LinkResourceInternal(FlightEditViewModel resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetEditViewModel(resource.Flight.GetAsInt("Id"))));
    }
}