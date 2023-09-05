using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Flights;

[ApiController]
public class FlightsController : HypermediaController
{
    private readonly GatewayRouter _router;

    public FlightsController(GatewayRouter router)
    {
        _router = router;
    }

    [HttpGet]
    [Route("/api/flight-management/flights/template")]
    public Task<IActionResult> GetCreateVm() => _router.ProxyAsync(HttpContext, "FlightManagement");

    [HttpPost]
    [Route("api/flight-management/flights")]
    public async Task<IActionResult> CreateFlight()
    {
        ContentResult? result = await _router.ProxyAsync(HttpContext, "FlightManagement") as ContentResult;
        double newEntityId = Convert.ToDouble(result?.Content);

        string newResourceUrl = Url.LinkTo<FlightsController>(c => c.GetEditVm(newEntityId));

        return Created(newResourceUrl, new string[] { newResourceUrl });
    }

    [HttpGet]
    [Route("api/views/fligths/search")]
    public async Task<IActionResult> GetSearchVm(string? from = null, string? to = null)
    {
        FlightSearchVm result = new FlightSearchVm { From = from, To = to };
        dynamic? flightSummary = await _router.GetAsync<DynamicResource>("FlightManagement", "/api/flight-management/flights/summary");
        result.FlightCount = flightSummary?.FlightCount ?? 0;

        // If no query param was provided, return an empty result
        if (!Request.Query.ContainsKey(nameof(from)) && !Request.Query.ContainsKey(nameof(to))) return Hypermedia(result);

        result.Flights = await _router.GetAsync<List<DynamicResource>>("FlightManagement", $"/api/flight-management/flights?from={from ?? ""}&to={to ?? ""}");

        // Iterate through all fligths and add price information
        foreach (dynamic flight in result.Flights)
        {
            dynamic? shoppingFlight = await _router.GetAsync<DynamicResource>("FlightShopping", $"/api/flight-shopping/flights/{flight.Id}");
            flight.Price = shoppingFlight?.Price;
        }

        return Hypermedia(result);
    }

    [HttpGet]
    [Route("api/views/fligths/{id}/edit")]
    public async Task<IActionResult> GetEditVm(double id)
    {
        dynamic result = await _router.GetAsync<FlightEditVm>("FlightManagement", $"/api/flight-management/flights/{id}");
        dynamic shoppingFlight = await _router.GetAsync<DynamicResource>("FlightShopping", $"/api/flight-shopping/flights/{id}");
        result.Price = shoppingFlight.Price;
        return Hypermedia(result);
    }
}