using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class FlightService
{
    private readonly FlightRepository _repository;

    public FlightService(FlightRepository flightRepository)
    {
        _repository = flightRepository;
    }
    
    public List<Flight> SearchFlights(FlightFilter flightFilter)
    => _repository.SearchFlights(flightFilter);
}
