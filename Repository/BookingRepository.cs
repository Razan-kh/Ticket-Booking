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
    }
    public void SaveBooking(Booking booking)
    {
        booking.BookingId = GenerateNumericId().ToString();
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private void SaveToCsv(Booking booking)
    {
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText(_filePath, line + Environment.NewLine);
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

    public List<Booking> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine(_filePath);
            Console.WriteLine("Warning: Booking file not found.");
            return [];
        }
        
        return File.ReadAllLines(_filePath)
            .Select(ParseBooking)
            .Where(b => b != null)
            .ToList()!;
    }
    private Booking? ParseBooking(string line)
    {
        var parts = line.Split(',');
        if (parts.Length < 5) return null;
        if (!double.TryParse(parts[4], out var price))
            return null;

        return new Booking
        {
            BookingId = parts[0],
            PassengerId = parts[1],
            FlightId = parts[2],
            Class = Enum.Parse<FlightClass>(parts[3]),
            Price = price
        };
    }
}
