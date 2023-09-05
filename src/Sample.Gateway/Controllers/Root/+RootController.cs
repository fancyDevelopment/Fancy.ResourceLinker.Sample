using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Root;

[ApiController]
public class RootController : HypermediaController
{
    [HttpGet]
    [Route("api")]
    public IActionResult GetRootVm()
    {
        return Hypermedia(new RootVm());
    }
}