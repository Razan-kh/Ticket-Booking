namespace Ticket_Booking.Models;
public class FlightFilter
{
    public string? DepartureCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public double? MaxPrice { get; set; }
    public FlightClass? ClassType { get; set; }
}