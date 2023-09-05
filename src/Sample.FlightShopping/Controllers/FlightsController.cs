using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Domain;
using FlightShopping.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightShopping.Controllers;

[ApiController]
public class FlightsController : HypermediaController
{
    private readonly FlightShoppingDbContext _dbContext;

    public FlightsController(FlightShoppingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("api/flight-shopping/flights/{id}")]
    public async Task<IActionResult> GetFlightById(int id)
    {
        Flight? flight = await _dbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);

        if(flight == null)
        {
            flight = new Flight { Id = id };
        }

        return Hypermedia(flight);
    }

    [HttpPut]
    [Route("api/flight-shopping/flights/{id}/price")]
    public async Task<IActionResult> UpdatePrice(int id, [FromBody] FlightPrice flightPrice)
    {
        Flight? flight = await _dbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);

        if (flight == null)
        {
            flight = new Flight { Id = id };
            _dbContext.Flights.Add(flight);
        }

        flight.Price = flightPrice;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}