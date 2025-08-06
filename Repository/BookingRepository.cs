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
    public List<Booking> GetByPassengerId(string passengerId) =>
        GetAll().Where(b => b.PassengerId == passengerId).ToList();
    public Booking? GetById(string bookingId) =>
        GetAll().FirstOrDefault(b => b.BookingId == bookingId);
}
