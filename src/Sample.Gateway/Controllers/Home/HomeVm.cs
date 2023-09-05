using Fancy.ResourceLinker.Models;

namespace Gateway.Controllers.Home;

public class HomeVm : ResourceBase
{
    public DynamicResource? FlightSummary { get; set; }
}