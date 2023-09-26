using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Domain;
using FlightShopping.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.FlightShopping.Dtos;
using Sample.FlightShopping.Infrastructure;

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
    [Route("api/flight-shopping/flights/summary", Name = nameof(GetFlightSummary))]
    [ProducesResponseType(typeof(FlightSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFlightSummary()
    {
        FlightSummaryDto result = new FlightSummaryDto();

        result.MaxBasePrice = await _dbContext.Flights.MaxAsync(f => f.Price!.BasePrice);
        result.MinBasePrice = await _dbContext.Flights.MinAsync(f => f.Price!.BasePrice);
        result.AveragePrice = await _dbContext.Flights.SumAsync(f => f.Price!.BasePrice) / await _dbContext.Flights.CountAsync();

        return Hypermedia(result);
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

    [HttpPost]
    [Route("api/flight-shopping/flights/")]
    public async Task<IActionResult> CreateFlight([FromBody] CreateFlightDto createFlightDto)
    {
        _dbContext.Flights.Add(new Flight { Id = createFlightDto.Id, Price = new FlightPrice() });

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}