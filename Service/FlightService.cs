namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using Ticket_Booking.Models;

public class FlightService
{
    public required FlightRepository Repository { get; init; }

    public List<Flight> SearchFlights(FlightFilter flightFilter)
    {
        return Repository.SearchFlights(flightFilter);
    }
}
