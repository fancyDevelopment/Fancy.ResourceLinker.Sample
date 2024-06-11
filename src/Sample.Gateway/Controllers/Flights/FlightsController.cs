using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Sample.Gateway.Controllers.Flights;

[ApiController]
public class FlightsController : ControllerBase
{
    private readonly GatewayRouter _router;

    public FlightsController(GatewayRouter router)
    {
        _router = router;
    }

    [HttpGet]
    [Route("api/views/flights/search")]
    public async Task<IActionResult> GetFlightSearchVm(string? from = null, string? to = null)
    {
        FlightSearchVm result = new FlightSearchVm();
        result.From = from;
        result.To = to;

        if(Request.Query.ContainsKey(nameof(from)) || Request.Query.ContainsKey(nameof(to)))
        {
            result.Flights = await _router.GetFlightManagementFlights<List<DynamicResource>>(from, to);

            foreach (dynamic flight in result.Flights)
            {
                dynamic shoppingFlight = await _router.GetFlightShoppingFlight<DynamicResource>((int)flight.Id);
                flight.Price = shoppingFlight.Price;
            }
        }
        
        return Ok(result);
    }
}
