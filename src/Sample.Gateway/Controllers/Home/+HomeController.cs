using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Home;

[ApiController]
public class HomeController : HypermediaController
{
    private readonly GatewayRouter _router;

    public HomeController(GatewayRouter router)
    {
        _router = router;
    }

    [HttpGet]
    [Route("/api/views/home")]
    public async Task<IActionResult> GetHomeVm()
    {
        HomeVm result = new HomeVm();
        result.FlightSummary = await _router.GetAsync<DynamicResource>("FlightManagement", "/api/flight-management/flights/summary");
        return Hypermedia(result);
    }
}