using Fancy.ResourceLinker.Hateoas;
using FlightManagement.Domain;
using FlightManagement.Dtos;
using FlightManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Controllers;

//[Authorize]
[ApiController]
public class FlightsController : HypermediaController
{
    private readonly ILogger<FlightsController> _logger;

    private readonly FlightManagementDbContext _flightManagementDbContext;

    public FlightsController(
        ILogger<FlightsController> logger, 
        FlightManagementDbContext flightManagementDbContext)
    {
        _logger = logger;
        _flightManagementDbContext = flightManagementDbContext;
    }

    [HttpGet]
    [Route("api/flight-management/flights/summary")]
    public async Task<IActionResult> GetFlightsSummary()
    {
        _logger.LogInformation("Reading flights summary.");
        IQueryable<Flight> query = _flightManagementDbContext.Flights;
        FlightsSummaryDto result = new FlightsSummaryDto();
        result.FlightCount = await _flightManagementDbContext.Flights.CountAsync();
        result.DeparturesCount = await _flightManagementDbContext.Flights.GroupBy(f => f.Connection.IcaoFrom).CountAsync();
        result.DestinationsCount = await _flightManagementDbContext.Flights.GroupBy(f => f.Connection.IcaoTo).CountAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("api/flight-management/flights")]
    public async Task<IActionResult> GetFlights(string? from = null, string? to = null)
    {
        _logger.LogInformation($"Getting flights from {from ?? "*"} to {to ?? "*"}");
        IQueryable<Flight> result = _flightManagementDbContext.Flights;
        if (!string.IsNullOrEmpty(from)) result = result.Where(f => f.Connection.From == from);
        if (!string.IsNullOrEmpty(to)) result = result.Where(f => f.Connection.To == to);
        return Hypermedia(await result.ToListAsync());
    }

    [HttpGet]
    [Route("api/flight-management/flights/template")]
    public IActionResult GetNewFlightTemplate()
    {
        CreateFlightDto template = new CreateFlightDto(
                                        new FlightConnection("", ""), 
                                        new FlightTimes(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(3)));

        template.Operator = new FlightOperator("", "");

        return Hypermedia(template);
    }

    [HttpGet]
    [Route("api/flight-management/flights/{id}")]
    public async Task<IActionResult> GetFlightById(int id)
    {
        Flight? result = await _flightManagementDbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);
        if (result == null) return NotFound();
        return Hypermedia(result);
    }

    [HttpPut]
    [Route("api/flight-management/flights/{id}/connection")]
    public async Task<IActionResult> UpdateFlightConnection(int id, [FromBody] FlightConnection connection)
    {
        Flight? flightToEdit = await _flightManagementDbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);
        if (flightToEdit == null) return BadRequest();
        flightToEdit.Connection = connection;

        await _flightManagementDbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("api/flight-management/flights/{id}/times")]
    public async Task<IActionResult> UpdateFlightTimes(int id, [FromBody] FlightTimes times)
    {
        Flight? flightToEdit = await _flightManagementDbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);
        if (flightToEdit == null) return BadRequest();
        flightToEdit.Times = times;

        await _flightManagementDbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("api/flight-management/flights/{id}/operator")]
    public async Task<IActionResult> UpdateFlightOperator(int id, [FromBody] FlightOperator @operator)
    {
        _logger.LogInformation($"Updating operator of flight with id {id}");

        Flight? flightToEdit = await _flightManagementDbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);
        if (flightToEdit == null) return BadRequest();
        flightToEdit.Operator = @operator;
        await _flightManagementDbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    [Route("api/flight-management/flights")]
    public async Task<IActionResult> CreateFlight([FromBody] CreateFlightDto createFlightDto)
    {
        _logger.LogInformation($"Creating a new flight.");
        Flight newFlight = new Flight(createFlightDto.Connection, createFlightDto.Times);
        newFlight.Operator = createFlightDto.Operator;
        _flightManagementDbContext.Flights.Add(newFlight);
        await _flightManagementDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFlightById), new { id = newFlight.Id }, newFlight.Id);
    }

    [HttpDelete]
    [Route("api/flight-management/flights/{id}")]
    public async Task<IActionResult> DeleteFlightById(int id)
    {
        Flight? flightToRemove = await _flightManagementDbContext.Flights.SingleOrDefaultAsync(f => f.Id == id);
        if (flightToRemove == null) return BadRequest();
        _flightManagementDbContext.Flights.Remove(flightToRemove);
        await _flightManagementDbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    [Route("api/flight-management/flights")]
    public async Task<IActionResult> DeleteAllFlights()
    {
        await _flightManagementDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Flights");
        return NoContent();
    }
}