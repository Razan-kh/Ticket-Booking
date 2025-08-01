// BookingService.cs
using Ticket_Booking.Models;
using Ticket_Booking.Repository;

public class BookingService 
{
    private readonly FlightRepository _flightRepo;
    private readonly BookingRepository _bookingRepo;

    public BookingService(FlightRepository flightRepo, BookingRepository bookingRepo)
    {
        _flightRepo = flightRepo;
        _bookingRepo = bookingRepo;
    }

    public void BookFlight(string passengerId, string flightId, FlightClass selectedClass)
    {  
        var flight = _flightRepo.GetFlightById(flightId);
        if (flight is null)
            throw new Exception("Flight not found");

        if (!flight.AvailableSeats.ContainsKey(selectedClass) || flight.AvailableSeats[selectedClass] <= 0)
            throw new Exception("No available seats for this class");
        flight.AvailableSeats[selectedClass]--;

        var price = flight.Prices[selectedClass];
        var booking = new Booking
        {
            BookingId = Guid.NewGuid().ToString(),
            PassengerId = passengerId,
            FlightId = flightId,
            Class = selectedClass,
            Price = price
        };

        _bookingRepo.SaveBooking(booking);
        _flightRepo.UpdateFlight(flight);
    }
}
