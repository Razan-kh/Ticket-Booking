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

    public List<string> BatchUploadFlights(string filePath)
    {
        var errors = _flightRepository.ReadFlightsFromCsv(filePath);
        return errors;
    }
    
    public Flight? GetFlightById(string flightId)
    => _flightRepository.GetFlightById(flightId);
}
