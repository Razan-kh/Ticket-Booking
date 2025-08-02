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
        {
            Console.WriteLine($"Flight with ID {flightId} not found.");
            return;
        }
        if (!flight.AvailableSeats.TryGetValue(selectedClass, out int value) || value <= 0)
            if (flight is null)
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
    
    public List<Booking> FilterBookings(
    string? flightId = null,
    double? price = null,
    string? departureCountry = null,
    string? destinationCountry = null,
    DateTime? departureDate = null,
    string? departureAirport = null,
    string? arrivalAirport = null,
    string? passengerId = null,
    FlightClass? flightClass = null)
    {
        var allBookings = _bookingRepo.GetAll();
        var allFlights = _flightRepo.GetAllFlights();

        var result = from booking in allBookings
                    join flight in allFlights on booking.FlightId equals flight.Id
                    where (string.IsNullOrEmpty(flightId) || booking.FlightId == flightId)
                        && (!price.HasValue || booking.Price == price)
                        && (string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry == departureCountry)
                        && (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry == destinationCountry)
                        && (!departureDate.HasValue || flight.DepartureDate.Date == departureDate.Value.Date)
                        && (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport == departureAirport)
                        && (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport == arrivalAirport)
                        && (string.IsNullOrEmpty(passengerId) || booking.PassengerId == passengerId)
                        && (!flightClass.HasValue || booking.Class == flightClass)
                    select booking;

                    return result.ToList();
    }

}
