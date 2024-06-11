using Fancy.ResourceLinker.Gateway.Routing;
using Fancy.ResourceLinker.Models;

namespace Sample.Gateway.Controllers;

public static class Routes
{
    public static Task<T> GetFlightManagementSummary<T>(this GatewayRouter router) where T : class
        => router.GetAsync<T>("FlightManagement", "/api/flight-management/flights/summary");

    public static Task<T> GetFlightShoppingSummary<T>(this GatewayRouter router) where T : class
        => router.GetAsync<T>("FlightShopping", "/api/flight-shopping/flights/summary");
}
