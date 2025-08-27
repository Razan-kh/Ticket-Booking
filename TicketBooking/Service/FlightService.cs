using System.Collections.Generic;
using TicketBooking.Models;
using TicketBooking.Repository;

namespace TicketBooking.Service;

public class FlightService
{
    private readonly IFlightRepository  _flightRepository;

    public FlightService(IFlightRepository  flightRepository)
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
