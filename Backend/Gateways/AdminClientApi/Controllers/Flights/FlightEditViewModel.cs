using Fancy.ResourceLinker.Models;

namespace Gateway.Controllers.Flights;

public class FlightEditViewModel : DynamicResourceBase
{
    public FlightEditViewModel(DynamicResource flight)
    {
        Flight = flight;
    }

    public DynamicResource Flight { get; set; }
}
