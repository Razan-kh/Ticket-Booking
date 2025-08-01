using System.Globalization;
using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class BookingRepository
{
    private List<Booking> _bookings = new();
    private readonly string _filePath;

    public BookingRepository(string filePath)
    {
        _filePath = filePath;
    }
    public void SaveBooking(Booking booking)
    {
        booking.BookingId = GenerateNumericId().ToString();
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private static void SaveToCsv(Booking booking)
    {
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText("Files/Bookings.csv", line + Environment.NewLine);
    }

    private int GenerateNumericId()
    {
        int maxId = 0;

        if (File.Exists(_filePath))
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length > 0 && int.TryParse(parts[0], out int id))
                {
                    if (id > maxId)
                        maxId = id;
                }
            }
        }

        return maxId + 1;
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
