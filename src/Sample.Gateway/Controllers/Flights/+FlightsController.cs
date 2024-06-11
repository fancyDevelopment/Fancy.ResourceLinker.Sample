using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Gateway.Controllers;
using System.Net;

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
    [Route("api/views/fligths/search")]
    public async Task<IActionResult> GetSearchViewModel(string? from = null, string? to = null)
    {
        var flightSummary = _router.GetFlightManagementFlightSummary<DynamicResource>();

        FlightSearchViewModel result = new FlightSearchViewModel { From = from, To = to };

        if (Request.Query.ContainsKey(nameof(from)) && Request.Query.ContainsKey(nameof(to)))
        {
            // Search for flights
            result.Flights = await _router.GetFlightManagementFlights<DynamicResource>(from, to);

            // Iterate through all flights and add price information
            await Task.WhenAll(result.Flights.Select(async (dynamic f) =>
            {
                try
                {
                    dynamic shoppingFlight = await _router.GetFlightShoppingFlight<DynamicResource>((int)f.Id);
                    f.Price = shoppingFlight.Price;
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.NotFound) f.Price = null;
                }
            }));
        }

        result.FlightCount = ((dynamic) await flightSummary).FlightCount ?? 0;

        return Hypermedia(result);
    }

    /// <summary>
    /// A sample actions which just passes throug a call to a specific backend.
    /// </summary>
    [HttpGet]
    [Route("/api/flight-management/flights/template")]
    public Task<IActionResult> GetCreateViewModel() => _router.ProxyAsync(HttpContext, "FlightManagement");

    /// <summary>
    /// A sample action which makes parallel requests to backends, deserializes the first result directly into a view model structure
    /// and merging other data into it dynamically making use of .net dynamic language runtime features.
    /// </summary>
    /// <param name="flightId">The flight identifier.</param>
    [HttpGet]
    [Route("api/views/flight/{flightId}/edit")]
    public async Task<IActionResult> GetEditViewModel(int flightId)
    {
        var managementFlight = _router.GetFlightManagementFlight<FlightEditViewModel>(flightId);
        var shoppingFlight = _router.GetFlightShoppingFlight<DynamicResource>(flightId);

        try
        {
            // Merge price value object into the result of the flight object from flight management
            dynamic result = await managementFlight;
            result.Price = ((dynamic) await shoppingFlight).Price;

            return Hypermedia(result);
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound) return NotFound();
            throw;
        }
    }

    [HttpPost]
    [Route("api/flight-management/flights")]
    public async Task<IActionResult> CreateFlight()
    {
        ContentResult? result = await _router.ProxyAsync(HttpContext, "FlightManagement") as ContentResult;
        int newEntityId = Convert.ToInt32(result?.Content);

        // This is just for demonstration purpose - in a real live application if you need to share the same key in two backends
        // you should do something like this with pattern of distributed transactions!
        await _router.PostAsync("FlightShopping", "/api/flight-shopping/flights", new { Id = newEntityId });

        string newResourceUrl = Url.LinkTo<FlightsController>(c => c.GetEditViewModel(newEntityId));

        return Created(newResourceUrl, new string[] { newResourceUrl });
    }
}