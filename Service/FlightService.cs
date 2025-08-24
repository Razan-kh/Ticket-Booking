using System.Collections.Generic;
using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class FlightService
{
    private readonly FlightRepository _flightRepository;

    public FlightService(FlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    
    public List<Flight> SearchFlights(FlightFilter flightFilter)
     => _flightRepository.SearchFlights(flightFilter);

    public Flight? GetFlightById(string flightId)
    => _flightRepository.GetFlightById(flightId);
}