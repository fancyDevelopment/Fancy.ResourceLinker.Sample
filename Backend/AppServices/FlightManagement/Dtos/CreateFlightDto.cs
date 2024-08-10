using Fancy.ResourceLinker.Models;
using FlightManagement.Domain;

namespace FlightManagement.Dtos;

public class CreateFlightDto : ResourceBase
{
    public FlightConnection Connection { get; set; }
    public FlightTimes Times { get; set; }
    public FlightOperator? Operator { get; set; }

    private CreateFlightDto()
    {
        Connection = null!;
        Times = null!;
    }

    public CreateFlightDto(FlightConnection connection, FlightTimes times)
    {
        Connection = connection;
        Times = times;
    }
}
