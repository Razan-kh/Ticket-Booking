using TicketBooking.Models;

namespace TicketBooking.Repository;

public interface IFlightRepository
{
    List<Flight> SearchFlights(FlightFilter filter);
    Flight? GetFlightById(string flightId);
    List<(string Field, string Type, string Constraints)> GetFlightValidationRules();
    List<string> ReadFlightsFromCsv(string filePath);
    public void UpdateFlight(Flight updatedFlight);
    public List<Flight> GetAllFlights();
}