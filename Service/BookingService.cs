using Ticket_Booking.Models;
using Ticket_Booking.Repository;

public class BookingService
{
    private readonly FlightRepository _flightRepo;
    private readonly BookingRepository _bookingRepo;

    public BookingService(FlightRepository flightRepository, BookingRepository bookingRepository)
    {
        _bookingRepo = bookingRepository;
        _flightRepo = flightRepository;
    }
    public void BookFlight(string passengerId, string flightId, FlightClass selectedClass)
    {
        var flight = _flightRepo.GetFlightById(flightId) ?? throw new Exception("Flight not found");
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

        _bookingRepo.SaveBooking(booking);
        _flightRepo.UpdateFlight(flight);
    }
       public void ModifyBooking(string bookingId, string newFlightId, FlightClass newClass)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking == null)
        {
            Console.WriteLine("Booking does not exist");
            return;
        }

        var newFlight = _flightRepo.GetFlightById(newFlightId);
        if (newFlight == null)
        {
            Console.WriteLine("Flight does not exist");
            return;
        }

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