using Fancy.ResourceLinker.Models;

namespace Sample.Gateway.Controllers.Flights;

public class FlightSearchVm
{
    public string? From { get; set; }

    public string? To { get; set; }

    public List<DynamicResource> Flights { get; set; } = new List<DynamicResource>();
}
