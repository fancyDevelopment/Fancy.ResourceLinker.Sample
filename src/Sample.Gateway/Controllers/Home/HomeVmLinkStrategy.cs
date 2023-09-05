using Microsoft.AspNetCore.Mvc;
using Fancy.ResourceLinker.Hateoas;

namespace Gateway.Controllers.Home;

public class HomeVmLinkStrategy : LinkStrategyBase<HomeVm>
{
    protected override void LinkResourceInternal(HomeVm resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<HomeController>(c => c.GetHomeVm()));
        resource.FlightSummary?.AddSocket("self", "http://localhost:5101/hubs/home", "updateFlightSummary", string.Empty);
    }
}