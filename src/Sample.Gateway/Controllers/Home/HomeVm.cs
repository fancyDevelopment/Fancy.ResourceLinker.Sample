using Fancy.ResourceLinker.Models;

namespace Sample.Gateway.Controllers.Home;

public class HomeVm
{
    public DynamicResource? FlightManagementSummary { get; set; }

    public DynamicResource? FlightShoppingSummary { get; set;  }
}
