using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class BookingRepository
{
    private List<Booking> _bookings = [];
    private readonly string _filePath;

    public BookingRepository(string filePath)
    {
        _filePath = filePath;
        _bookings = LoadBookings(_filePath);
    }

    public void SaveBooking(Booking booking)
    {
        booking.BookingId = _bookings.Count.ToString();
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private void SaveToCsv(Booking booking)
    {
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText(_filePath, line + Environment.NewLine);
    }

    public List<Booking> LoadBookings(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found");
            return [];
        }
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var parts = line.Split(',');
            if (parts.Length < 5)
            {
                Console.WriteLine("Invalid File Fprmat");
                continue;
            }
            if (!Enum.TryParse(parts[3], out FlightClass classType))
            {
                Console.WriteLine($"Invalid class type {parts[3]}");
                continue;
            }
            if (!double.TryParse(parts[4], out var price))
            {
                Console.WriteLine($"Invalid price {parts[4]}");
                continue;
            }
            var booking = new Booking
            {
                BookingId = parts[0],
                PassengerId = parts[1],
                FlightId = parts[2],
                Class = classType,
                Price = price
            };

            _bookings.Add(booking);
        }

        return _bookings;
    }
    public List<Booking> GetAllBookings => _bookings;
    public List<Booking> FilterBookings(BookingsFilter bookingsFilter, List<Flight> flights)
    {
        var result = from booking in _bookings
                 join flight in flights on booking.FlightId equals flight.Id
                 where (string.IsNullOrEmpty(bookingsFilter.FlightId) || booking.FlightId == bookingsFilter.FlightId)
                       && (!bookingsFilter.Price.HasValue || booking.Price == bookingsFilter.Price.Value)
                       && (string.IsNullOrEmpty(bookingsFilter.DepartureCountry) || flight.DepartureCountry == bookingsFilter.DepartureCountry)
                       && (string.IsNullOrEmpty(bookingsFilter.DestinationCountry) || flight.DestinationCountry == bookingsFilter.DestinationCountry)
                       && (!bookingsFilter.DepartureDate.HasValue || flight.DepartureDate.Date == bookingsFilter.DepartureDate.Value.Date)
                       && (string.IsNullOrEmpty(bookingsFilter.DepartureAirport) || flight.DepartureAirport == bookingsFilter.DepartureAirport)
                       && (string.IsNullOrEmpty(bookingsFilter.ArrivalAirport) || flight.ArrivalAirport == bookingsFilter.ArrivalAirport)
                       && (string.IsNullOrEmpty(bookingsFilter.PassengerId) || booking.PassengerId == bookingsFilter.PassengerId)
                       && (!bookingsFilter.FlightClass.HasValue || booking.Class == bookingsFilter.FlightClass)
                 select booking;

        return result.ToList();
    }
}
