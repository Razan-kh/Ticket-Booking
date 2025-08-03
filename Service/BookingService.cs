// BookingService.cs
using Ticket_Booking.Models;
using Ticket_Booking.Repository;

public class BookingService 
{
    public required FlightRepository FlightRepo { init; get; }
    public required BookingRepository BookingRepo { init; get; }

    public void BookFlight(string passengerId, string flightId, FlightClass selectedClass)
    {  
        var flight = FlightRepo.GetFlightById(flightId) ?? throw new Exception("Flight not found");
        if (!flight.AvailableSeats.TryGetValue(selectedClass, out int value) || value <= 0)
        {
            Console.WriteLine("No available seats");
            return;
        }
        flight.AvailableSeats[selectedClass] = --value;

        var price = flight.Prices[selectedClass];
        var booking = new Booking
        {
            BookingId = Guid.NewGuid().ToString(),
            PassengerId = passengerId,
            FlightId = flightId,
            Class = selectedClass,
            Price = price
        };

        BookingRepo.SaveBooking(booking);
        FlightRepo.UpdateFlight(flight);
    }
}
