using Fancy.ResourceLinker.Models;

namespace Sample.FlightShopping.Dtos;

public class FlightSummaryDto : ResourceBase
{
    public float MaxBasePrice { get; set; }

    public float MinBasePrice { get; set; }

    public float AveragePrice { get; set; }
}
