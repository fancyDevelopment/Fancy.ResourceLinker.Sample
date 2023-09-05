using Microsoft.AspNetCore.Mvc;
using Fancy.ResourceLinker.Hateoas;

namespace Gateway.Controllers.Flights;

public class FlightEditVmLinkStrategy : LinkStrategyBase<FlightEditVm>
{
    protected override void LinkResourceInternal(FlightEditVm resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetEditVm(resource.Id)));
    }
}