using Gateway.Controllers.Flights;
using Gateway.Controllers.Home;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Root;

public class RootLinkStrategy : LinkStrategyBase<RootViewModel>
{
    protected override void LinkResourceInternal(RootViewModel resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<RootController>(c => c.GetRootViewModel()));
        resource.AddLink("homeVm", urlHelper.LinkTo<HomeController>(c => c.GetHomeViewModel()));
        resource.AddLink("flightSearchVm", urlHelper.LinkTo<FlightsController>(c => c.GetSearchViewModel(null, null)));
        
        if(urlHelper.ActionContext.HttpContext.User.Identity?.IsAuthenticated ?? false)
            resource.AddLink("userinfo", urlHelper.LinkTo("userinfo"));
    }
}