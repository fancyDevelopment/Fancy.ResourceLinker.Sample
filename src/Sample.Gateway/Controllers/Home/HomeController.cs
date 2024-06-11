using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Gateway.Controllers.Home;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly GatewayRouter _router;

    public HomeController(GatewayRouter router)
    {
        _router = router;
    }

    [HttpGet]
    [Route("api/views/home")]
    public async Task<IActionResult> GetHomeVm()
    {
        var flightManagementSummary = _router.GetFlightManagementSummary<DynamicResource>();
        var flightShoppingSummary = _router.GetFlightShoppingSummary<DynamicResource>();

        HomeVm result = new()
        {
            FlightManagementSummary = await flightManagementSummary,
            FlightShoppingSummary = await flightShoppingSummary
        };

        return Ok(result);
    }
}
