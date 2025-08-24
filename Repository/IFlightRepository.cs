using TicketBooking.Models;

public interface IFlightRepository
{
    List<Flight> SearchFlights(FlightFilter filter);
}
