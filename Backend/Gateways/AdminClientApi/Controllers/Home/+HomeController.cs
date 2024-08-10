using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Mvc;
using Sample.Gateway.Controllers;

namespace Gateway.Controllers.Home;

[ApiController]
public class HomeController : HypermediaController
{
    private readonly GatewayRouter _router;

    public HomeController(GatewayRouter router)
    {
        _router = router;
    }

    /// <summary>
    /// A sample action which makes parallel requests to backends an merges the data into a new view model structure.
    /// </summary>
    [HttpGet]
    [Route("api/views/home")]
    public async Task<IActionResult> GetHomeViewModel()
    {
        var flightManagementSummary = _router.GetFlightManagementFlightSummary<DynamicResource>();
        var flightShoppingSummary = _router.GetFlightShoppingFlightSummary<DynamicResource>();

        HomeViewModel result = new HomeViewModel();

        result.FlightManagementSummary = await flightManagementSummary;
        result.FlightShoppingSummary = await flightShoppingSummary;

        return Hypermedia(result);
    }
}