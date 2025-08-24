namespace TicketBooking.Models;

public class BookingsFilter
{
    public string? FlightId { get; set; }
    public double? Price { get; set; }
    public string? DepartureCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? PassengerId { get; set; }
    public FlightClass? FlightClass { get; set; }
}