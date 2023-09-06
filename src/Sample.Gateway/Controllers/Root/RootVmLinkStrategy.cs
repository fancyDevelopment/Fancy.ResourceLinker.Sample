using Gateway.Controllers.Flights;
using Gateway.Controllers.Home;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Root;

public class RootVmLinkStrategy : LinkStrategyBase<RootVm>
{
    protected override void LinkResourceInternal(RootVm resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<RootController>(c => c.GetRootVm()));
        resource.AddLink("homeVm", urlHelper.LinkTo<HomeController>(c => c.GetHomeVm()));
        resource.AddLink("flightSearchVm", urlHelper.LinkTo<FlightsController>(c => c.GetSearchVm(null, null)));
        
        if(urlHelper.ActionContext.HttpContext.User.Identity?.IsAuthenticated ?? false)
            resource.AddLink("userinfo", "/userinfo");
    }
}