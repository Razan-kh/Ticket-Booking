namespace Ticket_Booking.Models;

public class Booking
{
    public required string BookingId { get; set; }
    public required string PassengerId { get; set; }
    public required string FlightId { get; set; }
    public required FlightClass Class { get; set; }
    public required double Price { get; set; }
    
    public override string ToString()
    {
        return @$"Booking Id : {BookingId}
Flight Id : {FlightId}
Class : {Class}
Price : {Price}";
    }

}
