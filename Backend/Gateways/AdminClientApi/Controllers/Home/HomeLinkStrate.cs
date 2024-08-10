using Microsoft.AspNetCore.Mvc;
using Fancy.ResourceLinker.Hateoas;

namespace Gateway.Controllers.Home;

public class HomeLinkStrate : LinkStrategyBase<HomeViewModel>
{
    protected override void LinkResourceInternal(HomeViewModel resource, IUrlHelper urlHelper)
    {
        resource.AddLink("self", urlHelper.LinkTo<HomeController>(c => c.GetHomeViewModel()));
    }
}