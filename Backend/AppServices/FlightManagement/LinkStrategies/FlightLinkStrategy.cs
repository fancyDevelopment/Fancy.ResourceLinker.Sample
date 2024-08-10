using Fancy.ResourceLinker.Hateoas;
using FlightManagement.Controllers;
using FlightManagement.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.LinkStrategies;

public class FlightLinkStrategy : LinkStrategyBase<Flight>
{
    protected override void LinkResourceInternal(Flight resource, IUrlHelper urlHelper)
    {
        //Add links
        resource.AddLink("self", urlHelper.LinkTo<FlightsController>(c => c.GetFlightById(resource.Id)));

        // Add actions
        if (urlHelper.ActionContext.HttpContext.User.IsInRole("flightmanagement-admin"))
        {
            resource.Connection.AddAction("update", "PUT", urlHelper.LinkTo<FlightsController>(c => c.UpdateFlightConnection(resource.Id, null!)));
            resource.Times.AddAction("update", "PUT", urlHelper.LinkTo<FlightsController>(c => c.UpdateFlightTimes(resource.Id, null!)));
            resource.Operator?.AddAction("update", "PUT", urlHelper.LinkTo<FlightsController>(c => c.UpdateFlightOperator(resource.Id, null!)));
        }

        resource.AddAction("delete", "DELETE", urlHelper.LinkTo<FlightsController>(c => c.DeleteFlightById(resource.Id)));
    }
}
