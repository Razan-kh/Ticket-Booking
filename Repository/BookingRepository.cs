using System.Globalization;
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
        var lines = File.ReadAllLines(filePath).Skip(1);

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

    public List<Booking> FilterBookings(BookingsFilter bookingFilter, List<Flight> flights)
    {
        var result = from booking in _bookings
                     join flight in flights on booking.FlightId equals flight.Id
                     select new { Booking = booking, Flight = flight };

        if (!string.IsNullOrEmpty(bookingFilter.FlightId))
            result = result.Where(entry => entry.Booking.FlightId == bookingFilter.FlightId);

        if (bookingFilter.Price.HasValue)
            result = result.Where(entry => entry.Booking.Price == bookingFilter.Price.Value);

        if (!string.IsNullOrEmpty(bookingFilter.DepartureCountry))
            result = result.Where(entry => entry.Flight.DepartureCountry == bookingFilter.DepartureCountry);

        if (!string.IsNullOrEmpty(bookingFilter.DestinationCountry))
            result = result.Where(entry => entry.Flight.DestinationCountry == bookingFilter.DestinationCountry);

        if (bookingFilter.DepartureDate.HasValue)
            result = result.Where(entry => entry.Flight.DepartureDate.Date == bookingFilter.DepartureDate.Value.Date);

        if (!string.IsNullOrEmpty(bookingFilter.DepartureAirport))
            result = result.Where(entry => entry.Flight.DepartureAirport == bookingFilter.DepartureAirport);

        if (!string.IsNullOrEmpty(bookingFilter.ArrivalAirport))
            result = result.Where(entry => entry.Flight.ArrivalAirport == bookingFilter.ArrivalAirport);

        if (!string.IsNullOrEmpty(bookingFilter.PassengerId))
            result = result.Where(entry => entry.Booking.PassengerId == bookingFilter.PassengerId);

        if (bookingFilter.FlightClass.HasValue)
            result = result.Where(entry => entry.Booking.Class == bookingFilter.FlightClass.Value);

        return result.Select(entry => entry.Booking).ToList();
    }

    public List<Booking> GetByPassengerId(string passengerId) =>
        _bookings.Where(b => b.PassengerId == passengerId).ToList();

    public void Update(Booking updated)
    {
        var bookings = GetAll();
        var index = bookings.FindIndex(b => b.BookingId == updated.BookingId);
        if (index == -1)
            throw new Exception("Booking not found.");

        bookings[index] = updated;
        File.WriteAllLines(_filePath, bookings.Select(SerializeBooking));
    }

    public List<Booking> GetAll()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException("Booking file not found.");

        return File.ReadAllLines(_filePath)
            .Select(ParseBooking)
            .Where(b => b != null)
            .ToList()!;
    }

    private Booking? ParseBooking(string line)
    {
        var parts = line.Split(',');
        if (parts.Length < 5) return null;

        if (!double.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            throw new FormatException($"Invalid price format: {parts[4]}");

        return new Booking
        {
            BookingId = parts[0],
            PassengerId = parts[1],
            FlightId = parts[2],
            Class = Enum.Parse<FlightClass>(parts[3]),
            Price = price
        };
    }
    private string SerializeBooking(Booking booking) =>
        $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        
    public Booking? GetById(string bookingId) =>
        GetAll().FirstOrDefault(b => b.BookingId == bookingId);
}
