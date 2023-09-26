using Fancy.ResourceLinker.Gateway.Routing;

namespace Sample.Gateway.Controllers
{
    public static class Routes
    {
        public static Task<T> GetFlightManagementFlightSummary<T>(this GatewayRouter router) where T : class => 
            router.GetCachedAsync<T>("FlightManagement", $"/api/flight-management/flights/summary", TimeSpan.FromSeconds(30));

        public static Task<T> GetFlightShoppingFlightSummary<T>(this GatewayRouter router) where T : class => 
            router.GetCachedAsync<T>("FlightShopping", $"/api/flight-shopping/flights/summary", TimeSpan.FromSeconds(30));

        public static Task<List<T>> GetFlightManagementFlights<T>(this GatewayRouter router, string? from, string? to) where T : class => 
            router.GetAsync<List<T>>("FlightManagement", $"/api/flight-management/flights?from={from ?? ""}&to={to ?? ""}");

        public static Task<T> GetFlightManagementFlight<T>(this GatewayRouter router, int flightId) where T : class => 
            router.GetAsync<T>("FlightManagement", $"/api/flight-management/flights/{flightId}");

        public static Task<T> GetFlightShoppingFlight<T>(this GatewayRouter router, int flightId) where T : class => 
            router.GetAsync<T>("FlightShopping", $"/api/flight-shopping/flights/{flightId}");
    }
}
