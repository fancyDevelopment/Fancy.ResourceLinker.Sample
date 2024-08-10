using Fancy.ResourceLinker.Models;

namespace Gateway.Controllers.Home;

public class HomeViewModel : ResourceBase
{
    public DynamicResource? FlightManagementSummary { get; set; }

    public DynamicResource? FlightShoppingSummary { get; set; }
}