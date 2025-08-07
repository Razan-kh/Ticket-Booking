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
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var parts = line.Split(',');
            if (parts.Length < 5)
                throw new FormatException($"Invalid booking record: {line}");

            if (!Enum.TryParse(parts[3], out FlightClass classType))
                throw new FormatException($"Invalid class type in booking: {parts[3]}");

            if (!double.TryParse(parts[4], out var price))
                throw new FormatException($"Invalid price in booking: {parts[4]}");

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
    public Booking? GetById(string bookingId) =>
        GetAll().FirstOrDefault(b => b.BookingId == bookingId);

    public List<Booking> GetAll() => _bookings;

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
    public List<Booking> GetByPassengerId(string passengerId) =>
        GetAll().Where(b => b.PassengerId == passengerId).ToList();
}
