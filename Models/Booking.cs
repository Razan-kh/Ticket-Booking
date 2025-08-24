namespace Ticket_Booking.Models;

public class Booking
{
    public required string BookingId { get; set; }
    public required string PassengerId { get; set; }
    public required string FlightId { get; set; }
    public required FlightClass Class { get; set; }
    public required double Price { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD

=======
    
>>>>>>> eaa622707cd7dd6ab15a8a3f2d331e162a440a17
    public override string ToString()
    {
        return @$"Booking Id : {BookingId}
Flight Id : {FlightId}
Class : {Class}
Price : {Price}";
    }
<<<<<<< HEAD
=======

>>>>>>> eaa622707cd7dd6ab15a8a3f2d331e162a440a17
}
=======
}
>>>>>>> UpdateBooking
