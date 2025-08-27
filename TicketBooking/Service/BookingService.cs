using TicketBooking.Models;
using TicketBooking.Repository;

namespace TicketBooking.Service;

public class BookingService
{
    private readonly IFlightRepository _flightRepo;
    private readonly IBookingRepository _bookingRepo;

    public BookingService(IFlightRepository flightRepository, IBookingRepository bookingRepository)
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

    public List<Booking> FilterBookings(BookingsFilter bookingsFilter)
    {
        var flights = _flightRepo.GetAllFlights();
        var bookings = _bookingRepo.FilterBookings(bookingsFilter, flights);
        return bookings;
    }

    public void UpdateOne(string bookingId, string newFlightId, FlightClass newClass)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking is null)
        {
            Console.WriteLine("Booking does not exist");
            return;
        }

        var newFlight = _flightRepo.GetFlightById(newFlightId);
        if (newFlight is null)
        {
            Console.WriteLine("Flight does not exist");
            return;
        }
        if (!newFlight.AvailableSeats.TryGetValue(newClass, out int value) || value <= 0)
        {
            Console.WriteLine("No available seats");
            return;
        }
        newFlight.AvailableSeats[newClass] = --value;
        booking.FlightId = newFlightId;
        booking.Class = newClass;
        booking.Price = newFlight.Prices[newClass];
        _flightRepo.UpdateFlight(newFlight);
        _bookingRepo.UpdateOne(booking);
    }

    public List<Booking> GetBookingsForPassenger(string passengerId)
    => _bookingRepo.GetByPassengerId(passengerId);

    public void CancelBooking(string bookingId)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking is null)
        {
            Console.WriteLine("Booking id is wrong");
            return;
        }
        _bookingRepo.DeleteOne(bookingId);
    }
}
