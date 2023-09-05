using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Domain;
using FlightShopping.Dtos;
using FlightShopping.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightShopping.Controllers;

[ApiController]
public class ShoppingBasketController : HypermediaController
{
    private readonly FlightShoppingDbContext _dbContext;

    public ShoppingBasketController(FlightShoppingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("api/flight-shopping/shopping-baskets/current")]
    public async Task<IActionResult> GetCurrentShoppingBasket()
    {
        ShoppingBasket basket = await GetShoppingBasketOfCurrentUserAsync(User?.Identity?.Name);
        return Hypermedia(basket);
    }

    [HttpPost]
    [Route("api/flight-shopping/shopping-baskets/current/tickets")]
    public async Task<IActionResult> AddTicket([FromBody] AddTicketDto addTicketDto)
    {
        ShoppingBasket basket = await GetShoppingBasketOfCurrentUserAsync(User?.Identity?.Name);

        Flight flight = await _dbContext.Flights.SingleAsync(f => f.Id == addTicketDto.FlightId);

        if (flight.Price == null) return Forbid("Flight not ready for shopping!");

        // Calculate ticket price
        float basePrice = flight.Price.BasePrice;
        float seatReservationSurcharge = 0.0f;
        float classSurcharge = 0.0f;

        if (addTicketDto.SeatNumber != null)
        {
            seatReservationSurcharge = flight.Price.SeatReservationSurcharge;
        }

        Ticket newTicket = new Ticket(flight.Id, flight.Price.BasePrice, seatReservationSurcharge, classSurcharge);
        basket.Tickets!.Add(newTicket);
        basket.TotalBasketPrice += newTicket.TotalTicketPrice;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("api/flight-shopping/shopping-baskets/current/tickets/{id}")]
    public async Task<IActionResult> DeleteTicketFromBasket(int id)
    {
        ShoppingBasket shoppingBasket = await GetShoppingBasketOfCurrentUserAsync(User?.Identity?.Name);

        Ticket? ticketToDelete = shoppingBasket.Tickets!.SingleOrDefault(ticket => ticket.Id == id);

        if (ticketToDelete == null) return NotFound("Ticket not found!");

        shoppingBasket.Tickets?.Remove(ticketToDelete);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut]
    [Route("api/flight-shopping/shopping-baskets/current/payment-details")]
    public async Task<IActionResult> UpdatePaymentDetails([FromBody] ShoppingBasketPaymentDetails paymentDetails)
    {
        ShoppingBasket shoppingBasket = await GetShoppingBasketOfCurrentUserAsync(User?.Identity?.Name);
        shoppingBasket.PaymentDetails = paymentDetails;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    [Route("api/flight-shopping/shopping-baskets/ordered")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutDto checkoutDto)
    {
        ShoppingBasket? shoppingBasket = _dbContext.ShoppingBaskets.SingleOrDefault(sb => sb.Id == checkoutDto.ShoppingBasketId);

        if (shoppingBasket == null) return NotFound("Shopping basket not found!");

        if (shoppingBasket.State != ShoppingBasketState.Current) return Forbid("Shopping basket can to be checked out!");

        shoppingBasket.State = ShoppingBasketState.Ordered;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private async Task<ShoppingBasket> GetShoppingBasketOfCurrentUserAsync(string? currentUserId)
    {
        currentUserId = currentUserId ?? "unkown";
        ShoppingBasket? shoppingBasket = await _dbContext.ShoppingBaskets
                                                         .Include(sb => sb.Tickets)
                                                         .SingleOrDefaultAsync(sb => sb.UserId == currentUserId && sb.State == ShoppingBasketState.Current);

        if (shoppingBasket == null)
        {
            shoppingBasket = new ShoppingBasket(currentUserId);
            _dbContext.ShoppingBaskets.Add(shoppingBasket);
            _dbContext.SaveChanges();
        }

        return shoppingBasket;
    }
}