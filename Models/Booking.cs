namespace Ticket_Booking.Models;

public class Booking
{
    public required string BookingId { get; set; }
    public required string PassengerId { get; set; }
    public required string FlightId { get; set; }
    public required FlightClass Class { get; set; }
    public required double Price { get; set; }
}