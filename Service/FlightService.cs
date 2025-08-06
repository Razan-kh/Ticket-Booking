namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using Ticket_Booking.Models;

public class FlightService
{
    private readonly FlightRepository _flightRepository;

    public FlightService(FlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
   
    public List<Flight> SearchFlights(FlightFilter flightFilter)
    {
        return _flightRepository.SearchFlights(flightFilter);
    }

    public Flight? GetFlightById(string flightId)
    {
        Flight? flight=_flightRepository.GetFlightById(flightId);
        return flight;
    }
}
