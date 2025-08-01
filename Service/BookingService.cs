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
        var flight = _flightRepo.GetFlightById(flightId) ?? throw new Exception("Flight not found");
        if (!flight.AvailableSeats.TryGetValue(selectedClass, out int value) || value <= 0)
            throw new Exception("No available seats for this class");
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

        _bookingRepo.SaveBooking(booking);
        _flightRepo.UpdateFlight(flight);
    }
       public void ModifyBooking(string bookingId, string newFlightId, FlightClass newClass)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking == null)
            throw new Exception("Booking not found.");

        var newFlight = _flightRepo.GetAllFlights().FirstOrDefault(f => f.Id == newFlightId);
        if (newFlight == null)
            throw new Exception("New flight not found.");

        booking.FlightId = newFlightId;
        booking.Class = newClass;
        booking.Price = newFlight.Prices[newClass];

        _bookingRepo.Update(booking);
    }

    public List<Booking> GetBookingsForPassenger(string passengerId)
    {
        return _bookingRepo.GetByPassengerId(passengerId);
    }
}
