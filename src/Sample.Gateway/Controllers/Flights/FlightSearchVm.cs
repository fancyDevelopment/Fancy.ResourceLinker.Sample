using Fancy.ResourceLinker.Models;

namespace Gateway.Controllers.Flights;

public class FlightSearchVm : ResourceBase
{
    public double FlightCount { get; set; }

    public string? From { get; set; }

    public string? To { get; set; }

    public List<DynamicResource> Flights { get;set; } = new List<DynamicResource>();
}