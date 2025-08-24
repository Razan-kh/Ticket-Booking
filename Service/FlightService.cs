using System.Collections.Generic;
using TicketBooking.Models;

namespace TicketBooking.Repository;

public class FlightService
{
    private readonly FlightRepository _flightRepository;

    public FlightService(FlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    
    public List<Flight> SearchFlights(FlightFilter flightFilter)
     => _flightRepository.SearchFlights(flightFilter);

    public List<string> BatchUploadFlights(string filePath)
    {
        var errors = _flightRepository.ReadFlightsFromCsv(filePath);
        return errors;
    }

    public List<(string Field, string Type, string Constraints)> ValidationInfo()
    => _flightRepository.GetFlightValidationRules();

    public Flight? GetFlightById(string flightId)
    =>_flightRepository.GetFlightById(flightId);
}
