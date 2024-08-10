using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fancy.ResourceLinker.Models;

namespace FlightManagement.Domain;

public class Flight : ResourceBase
{
    private Flight() 
    {
        Connection = null!;
        Times = null!;
    }

    public Flight(FlightConnection connection, FlightTimes times)
    {
        Connection = connection;
        Times = times;
    }

    public int Id { get; set; }

    public FlightConnection Connection { get; set; }
    public FlightTimes Times { get; set; }
    public FlightOperator? Operator { get; set; }

    [NotMapped]
    public TimeSpan FlightDuration => Times.Landing.Subtract(Times.TakeOff);
}

public class FlightConnection : ResourceBase
{
    public FlightConnection(string from, string to)
    {
        From = from;
        To = to;
    }

    private FlightConnection() 
    {
        From = null!;
        To = null!;
    }

    public string From { get; set; }
    public string To { get; set; }
    public string? IcaoFrom { get; set; }
    public string? IcaoTo { get; set; }
}

public class FlightTimes : ResourceBase
{
    public FlightTimes(DateTime takeOff, DateTime landing)
    {
        TakeOff = takeOff;
        Landing = landing;
    }

    private FlightTimes() { }

    public DateTime TakeOff { get; set; }
    public DateTime Landing { get; set; }
}

public class FlightOperator : ResourceBase
{
    public FlightOperator(string name, string shortName)
    {
        Name = name;
        ShortName = shortName;
    }

    private FlightOperator() 
    {
        Name = null!;
        ShortName = null!;
    }

    public string Name { get; set; }
    public string ShortName { get; set; }

    public int SeatCount { get; set; }
}